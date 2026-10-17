// Run with Unity CLI eval_file while this project's Editor is idle.
// Rebuilds only the two existing clips, preserving their GUIDs and controller.
string directory="Assets/Animations/Player/CC0Rifle";
if(UnityEditor.EditorApplication.isPlaying||UnityEditor.EditorApplication.isCompiling||UnityEditor.EditorApplication.isUpdating)
 throw new System.InvalidOperationException("An idle Editor is required.");
var input=Newtonsoft.Json.Linq.JObject.Parse(System.IO.File.ReadAllText("Tools/AnimationBaking/RiflePoses.json"));
if(input["format"].ToObject<int>()!=1)throw new System.InvalidOperationException("Unsupported pose format.");
string avatarGuid=input["targetAvatarGuid"].ToObject<string>();
foreach(string team in new[]{"Blue","Red"}){
 var prefab=UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.GameObject>("Assets/Resources/Prefabs/Player/Soldier"+team+".prefab");
 var animator=prefab.GetComponentInChildren<UnityEngine.Animator>(true);
 if(UnityEditor.AssetDatabase.AssetPathToGUID(UnityEditor.AssetDatabase.GetAssetPath(animator.avatar))!=avatarGuid)
  throw new System.InvalidOperationException("The curated poses require the original Military Avatar.");
}
var results=new System.Collections.Generic.List<object>();
foreach(bool idle in new[]{true,false}){
 string name=idle?"Idle_Aiming":"Death_Front",path=directory+"/"+name+".anim";
 var clip=UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.AnimationClip>(path);
 if(clip==null||clip.legacy||!clip.humanMotion)throw new System.InvalidOperationException("Existing Humanoid clip required: "+path);
 var data=input[idle?"idle":"death"];float duration=data["duration"].ToObject<float>();
 var bindings=new System.Collections.Generic.List<UnityEditor.EditorCurveBinding>();
 var curves=new System.Collections.Generic.List<UnityEngine.AnimationCurve>();
 for(int i=0;i<UnityEngine.HumanTrait.MuscleCount+7;i++){
  bool muscle=i<UnityEngine.HumanTrait.MuscleCount;string property;
  if(muscle){
   property=UnityEngine.HumanTrait.MuscleName[i];var parts=property.Split(' ');
   if(parts.Length>=3&&new[]{"Thumb","Index","Middle","Ring","Little"}.Contains(parts[1]))
    property=parts[0]+"Hand."+parts[1]+"."+string.Join(" ",parts.Skip(2));
  }else property=new[]{"RootT.x","RootT.y","RootT.z","RootQ.x","RootQ.y","RootQ.z","RootQ.w"}[i-UnityEngine.HumanTrait.MuscleCount];
  var curve=new UnityEngine.AnimationCurve();
  if(idle){
   float value=muscle?data["muscles"][i].ToObject<float>():i<UnityEngine.HumanTrait.MuscleCount+3?data["bodyPosition"][i-UnityEngine.HumanTrait.MuscleCount].ToObject<float>():data["bodyRotation"][i-UnityEngine.HumanTrait.MuscleCount-3].ToObject<float>();
   if(muscle){int intervals=data["intervals"].ToObject<int>();for(int k=0;k<=intervals;k++)curve.AddKey(new UnityEngine.Keyframe(duration*k/intervals,value+(property==data["breathingMuscle"].ToObject<string>()?data["breathingAmplitude"].ToObject<float>()*UnityEngine.Mathf.Sin(2f*UnityEngine.Mathf.PI*k/intervals):0f)));}
   else curve=UnityEngine.AnimationCurve.Linear(0f,value,duration,value);
  }else{
   float? previous=null;int unwrap=data["unwrapMuscleIndex"].ToObject<int>();float period=data["unwrapPeriod"].ToObject<float>();
   foreach(var record in data["records"]){
    float value=muscle?record["muscles"][i].ToObject<float>():i<UnityEngine.HumanTrait.MuscleCount+3?record["bodyPosition"][i-UnityEngine.HumanTrait.MuscleCount].ToObject<float>():record["bodyRotation"][i-UnityEngine.HumanTrait.MuscleCount-3].ToObject<float>();
    // Left forearm limits are +/-90 degrees on this Avatar. A period of four
    // muscle units is a full turn, verified to preserve every sampled bone.
    if(i==unwrap&&previous.HasValue){while(value-previous.Value>period*.5f)value-=period;while(value-previous.Value< -period*.5f)value+=period;}
    previous=value;curve.AddKey(new UnityEngine.Keyframe(record["seconds"].ToObject<float>(),value));
   }
   for(int k=0;k<curve.length;k++){UnityEditor.AnimationUtility.SetKeyLeftTangentMode(curve,k,UnityEditor.AnimationUtility.TangentMode.Linear);UnityEditor.AnimationUtility.SetKeyRightTangentMode(curve,k,UnityEditor.AnimationUtility.TangentMode.Linear);}
  }
  bindings.Add(UnityEditor.EditorCurveBinding.FloatCurve("",typeof(UnityEngine.Animator),property));curves.Add(curve);
 }
 clip.frameRate=data["frameRate"].ToObject<float>();clip.ClearCurves();UnityEditor.AnimationUtility.SetEditorCurves(clip,bindings.ToArray(),curves.ToArray());clip.EnsureQuaternionContinuity();
 var settings=UnityEditor.AnimationUtility.GetAnimationClipSettings(clip);settings.startTime=0;settings.stopTime=duration;settings.loopTime=idle;settings.loopBlend=idle;settings.loopBlendOrientation=true;settings.loopBlendPositionY=true;settings.loopBlendPositionXZ=true;settings.keepOriginalOrientation=true;settings.keepOriginalPositionY=true;settings.keepOriginalPositionXZ=true;UnityEditor.AnimationUtility.SetAnimationClipSettings(clip,settings);
 UnityEditor.AssetDatabase.SaveAssetIfDirty(clip);
 // Keep the generated YAML free of serializer-only trailing blanks.
 var serializedText=System.IO.File.ReadAllText(path);
 var cleanText=System.Text.RegularExpressions.Regex.Replace(serializedText,@"[ \t]+(?=\r?$)","",System.Text.RegularExpressions.RegexOptions.Multiline);
 System.IO.File.WriteAllText(path,cleanText,new System.Text.UTF8Encoding(false));
 results.Add(new {path=path,guid=UnityEditor.AssetDatabase.AssetPathToGUID(path),human=clip.humanMotion,length=clip.length,loop=clip.isLooping,bindings=UnityEditor.AnimationUtility.GetCurveBindings(clip).Length});
}
return new {regenerated=results,prefabsOrControllerChanged=false};
