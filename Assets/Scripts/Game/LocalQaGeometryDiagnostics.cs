#if UNITY_ANDROID && SUDDEN_FORCE_LOCAL_QA
using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;
public sealed class LocalQaGeometryDiagnostics : MonoBehaviour
{
const string Game="Assets/Scenes/Game/DesertHouse.unity";
const string Protocol="sff-q20261022-ladder-v1-intro1-riflecore2-gaitprod1-window1-shoulderref1-localqa";
const BindingFlags Fields=BindingFlags.Instance|BindingFlags.NonPublic;
static LocalQaGeometryDiagnostics _self;
bool _owned,_done,_bound;
int _count,_frame=-1;
double _until,_time=-1,_readyUntil=-1;
[Serializable] sealed class Row
{
public string error,canvasRenderMode;
public int frame,width,height,sample,cachedWidth,cachedHeight;
public Rect safeArea,pixelRect,parentScreenRect,cachedSafeArea;
public Vector2 crosshairScreenCentre,cameraPixelCentre,crosshairMinusCameraCentre;
public Vector2 crossAnchorMin,crossAnchorMax,crossOffsetMin,crossOffsetMax,crossAnchoredPosition;
public Vector2 parentAnchorMin,parentAnchorMax,parentOffsetMin,parentOffsetMax;
public Vector3 centreRayOrigin,centreRayDirection,simulationShotOrigin;
public bool parentSafeRectFound,cacheAvailable,simulationShotOriginRead;
}
static bool Scope()=>Application.identifier=="com.AeDeong.SuddenForceFPS.localqa"&&Application.version=="1.0.197-qa8";
[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
static void Begin()
{
if(!Scope()||_self!=null)return;
var go=new GameObject("SFF_QA8_GeometryOwned");
go.hideFlags=HideFlags.HideAndDontSave;DontDestroyOnLoad(go);
_self=go.AddComponent<LocalQaGeometryDiagnostics>();
_self._owned=true;_self._until=Time.realtimeSinceStartupAsDouble+300;
Canvas.willRenderCanvases+=_self.Observe;
}
void Update(){if(!_owned||_done)return;double now=Time.realtimeSinceStartupAsDouble;if(!Scope()||now>=_until)Finish("ttl-or-scope");else if(!_bound&&_readyUntil>=0&&now>=_readyUntil)Finish("readiness-timeout");else if(_bound&&SceneManager.GetActiveScene().path!=Game)Finish("left-game");}
void OnDisable(){if(_owned)Finish(null);}
void OnDestroy(){Canvas.willRenderCanvases-=Observe;if(_self==this)_self=null;}
void Finish(string error)
{
if(_done)return;_done=true;Canvas.willRenderCanvases-=Observe;
try{if(error!=null&&_count<8)Log(new Row{error=error,frame=Time.frameCount,sample=_count});}
finally{if(_self==this)_self=null;if(_owned)Destroy(gameObject);}
}
bool Ready(bool ready,string error,double now){if(ready)return true;if(_bound||(_readyUntil>=0&&now>=_readyUntil))Finish(error);return false;}
static void Log(Row row)=>Debug.Log("[SFF_QA8_GEOMETRY] "+JsonUtility.ToJson(row));
static Rect ScreenRect(RectTransform rect,Camera cam)
{
var corners=new Vector3[4];rect.GetWorldCorners(corners);
var min=new Vector2(float.PositiveInfinity,float.PositiveInfinity);var max=-min;
foreach(var point in corners){var p=RectTransformUtility.WorldToScreenPoint(cam,point);min=Vector2.Min(min,p);max=Vector2.Max(max,p);}
return Rect.MinMaxRect(min.x,min.y,max.x,max.y);
}
void Observe()
{
if(_done||!_owned||!Scope())return;
double now=Time.realtimeSinceStartupAsDouble;int frame=Time.frameCount;
if(now>=_until){Finish("ttl");return;}
if(frame==_frame||now-_time<.25||SceneManager.GetActiveScene().path!=Game)return;
_frame=frame;_time=now;
try
{
GamePlayerNetworkData own=null;int count=0;
foreach(var data in FindObjectsByType<GamePlayerNetworkData>(FindObjectsSortMode.None))
if(data.isActiveAndEnabled&&data.gameObject.scene.path==Game&&data.Object!=null&&data.Object.IsValid&&data.Object.HasInputAuthority&&data.Runner!=null&&data.Runner.IsRunning){own=data;count++;}
if(count>1){Finish("own-input-count");return;}
if(count==1&&_readyUntil<0)_readyUntil=now+3;
if(!Ready(count==1,"own-input-count",now))return;
var manager=NetworkRunnerManager.Instance;
var settings=manager!=null?typeof(NetworkRunnerManager).GetField("_sceneAppSettings",Fields)?.GetValue(manager) as Fusion.Photon.Realtime.FusionAppSettings:null;
if(settings!=null&&settings.AppVersion!=null&&settings.AppVersion!=Protocol){Finish("wrong-namespace");return;}
if(!Ready(manager!=null&&!manager.IsRetired&&manager.SessionPhase==NetworkSessionPhase.Game&&settings!=null&&settings.AppVersion==Protocol,"namespace-or-phase",now))return;
var cam=typeof(GamePlayerNetworkData).GetField("_localCamera",Fields)?.GetValue(own) as Camera;
int mainCount=0;foreach(var active in Camera.allCameras)if(active.CompareTag("MainCamera"))mainCount++;
if(mainCount>1){Finish("main-camera-count");return;}
if(!Ready(cam!=null&&cam.isActiveAndEnabled&&cam.transform.IsChildOf(own.transform)&&Camera.main==cam&&mainCount==1,"camera-mismatch",now))return;
RectTransform cross=null;count=0;
foreach(var rect in FindObjectsByType<RectTransform>(FindObjectsSortMode.None))if(rect.name=="ImgCrossHair"&&rect.gameObject.activeInHierarchy&&rect.gameObject.scene.path==Game){cross=rect;count++;}
if(count>1){Finish("crosshair-count");return;}
if(!Ready(count==1,"crosshair-count",now))return;
var canvas=cross.GetComponentInParent<Canvas>();var safe=cross.GetComponentInParent<ScreenSafeArea>();
var parent=safe!=null?safe.transform as RectTransform:cross.parent as RectTransform;
if(!Ready(canvas!=null&&canvas.isActiveAndEnabled&&parent!=null,"canvas-or-parent",now))return;
canvas=canvas.rootCanvas;if(!Ready(canvas!=null&&canvas.isActiveAndEnabled,"root-canvas",now))return;Camera ui=canvas.renderMode==RenderMode.ScreenSpaceOverlay?null:canvas.worldCamera;
if(!Ready(canvas.renderMode==RenderMode.ScreenSpaceOverlay||ui==cam,"canvas-camera-mismatch",now))return;
var centre=RectTransformUtility.WorldToScreenPoint(ui,cross.TransformPoint(cross.rect.center));var pixel=cam.pixelRect;
if(!Ready(Screen.width>0&&Screen.height>0&&pixel.width>0&&pixel.height>0,"screen-geometry",now))return;
_bound=true;
var ray=cam.ScreenPointToRay(new Vector3(pixel.center.x,pixel.center.y,0));
var row=new Row{frame=frame,width=Screen.width,height=Screen.height,sample=++_count,safeArea=Screen.safeArea,pixelRect=pixel,canvasRenderMode=canvas.renderMode.ToString(),crosshairScreenCentre=centre,cameraPixelCentre=pixel.center,crosshairMinusCameraCentre=centre-pixel.center,crossAnchorMin=cross.anchorMin,crossAnchorMax=cross.anchorMax,crossOffsetMin=cross.offsetMin,crossOffsetMax=cross.offsetMax,crossAnchoredPosition=cross.anchoredPosition,parentScreenRect=ScreenRect(parent,ui),parentAnchorMin=parent.anchorMin,parentAnchorMax=parent.anchorMax,parentOffsetMin=parent.offsetMin,parentOffsetMax=parent.offsetMax,parentSafeRectFound=safe!=null,centreRayOrigin=ray.origin,centreRayDirection=ray.direction,simulationShotOrigin=own.SimulationShotOrigin,simulationShotOriginRead=true};
if(safe!=null){var type=typeof(ScreenSafeArea);var area=type.GetField("_lastArea",Fields);var w=type.GetField("_width",Fields);var h=type.GetField("_height",Fields);if(area!=null&&w!=null&&h!=null){row.cachedSafeArea=(Rect)area.GetValue(safe);row.cachedWidth=(int)w.GetValue(safe);row.cachedHeight=(int)h.GetValue(safe);row.cacheAvailable=true;}}
Log(row);if(_count>=8)Finish(null);
}
catch(Exception){Finish("observation-error");}
}
}
#endif
