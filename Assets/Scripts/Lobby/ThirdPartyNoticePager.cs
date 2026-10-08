using System;
using System.Collections.Generic;
using System.Globalization;

/// <summary>Lossless source ranges for a bounded plain-text notice reader.</summary>
public sealed class ThirdPartyNoticePager
{
    public readonly struct Page
    {
        public readonly int Start;
        public readonly int Length;
        public readonly int DisplayLength;

        public Page(int start, int length, int displayLength)
        {
            Start = start;
            Length = length;
            DisplayLength = displayLength;
        }
    }

    private readonly string _source;
    private readonly List<Page> _pages = new();
    public int Count => _pages.Count;
    public Page this[int index] => _pages[index];

    public ThirdPartyNoticePager(string source, int maxCharacters = 4096, int maxLineBreaks = 96)
    {
        if (maxCharacters < 2) throw new ArgumentOutOfRangeException(nameof(maxCharacters));
        if (maxLineBreaks < 1) throw new ArgumentOutOfRangeException(nameof(maxLineBreaks));
        _source = source ?? string.Empty;
        for (var start = 0; start < _source.Length;)
        {
            var end = FindEnd(start, maxCharacters, maxLineBreaks);
            var displayLength = end - start;
            // A form feed is retained in the source range, but acts as a page break.
            if (_source[end - 1] == '\f') displayLength--;
            _pages.Add(new Page(start, end - start, displayLength));
            start = end;
        }
    }

    public string GetDisplayText(int index)
    {
        var page = _pages[index];
        return _source.Substring(page.Start, page.DisplayLength);
    }

    public string GetSourceText(int index)
    {
        var page = _pages[index];
        return _source.Substring(page.Start, page.Length);
    }

    private int FindEnd(int start, int maxCharacters, int maxLineBreaks)
    {
        var acceptedEnd = start;
        var lineBreaks = 0;
        var lastLineEnd = start;
        var lastParagraphEnd = start;
        var elements = StringInfo.GetTextElementEnumerator(_source, start);
        while (elements.MoveNext())
        {
            var end = elements.ElementIndex;
            if (end == start || !IsSafeBoundary(_source, end)) continue;
            if (!Accept(end)) return PreferredEnd();
            if (_source[acceptedEnd - 1] == '\f') return acceptedEnd;
        }
        if (Accept(_source.Length)) return acceptedEnd;
        return PreferredEnd();

        bool Accept(int end)
        {
            var count = lineBreaks;
            for (var i = acceptedEnd; i < end; i++)
            {
                if (_source[i] == '\f')
                {
                    end = i + 1;
                    break;
                }
                if (_source[i] == '\n') count++;
            }
            if (end - start > maxCharacters || count > maxLineBreaks) return false;
            for (var i = acceptedEnd; i < end; i++)
            {
                if (_source[i] != '\n') continue;
                if (i == lastLineEnd || (i == lastLineEnd + 1 && _source[lastLineEnd] == '\r'))
                    lastParagraphEnd = i + 1;
                lastLineEnd = i + 1;
            }
            acceptedEnd = end;
            lineBreaks = count;
            return true;
        }

        int PreferredEnd()
        {
            if (acceptedEnd == start)
                throw new InvalidOperationException("A notice text element exceeds the page budget.");
            var halfway = start + (acceptedEnd - start) / 2;
            if (lastParagraphEnd > start && lastParagraphEnd >= halfway) return lastParagraphEnd;
            if (lastLineEnd > start) return lastLineEnd;
            return acceptedEnd;
        }
    }

    private static bool IsSafeBoundary(string text, int index)
    {
        if (index <= 0 || index >= text.Length) return true;
        if (char.IsHighSurrogate(text[index - 1]) && char.IsLowSurrogate(text[index])) return false;
        var next = char.ConvertToUtf32(text, index);
        var previousIndex = index - 1;
        if (char.IsLowSurrogate(text[previousIndex]) && previousIndex > 0
            && char.IsHighSurrogate(text[previousIndex - 1])) previousIndex--;
        var previous = char.ConvertToUtf32(text, previousIndex);
        if (previous == '\r' && next == '\n') return false;
        // Text controls end a grapheme; a form feed must remain a separate page token.
        if (previous == '\f' || next == '\f' || previous == '\n' || next == '\n'
            || previous == '\r' || next == '\r') return true;
        var category = CharUnicodeInfo.GetUnicodeCategory(text, index);
        if (category == UnicodeCategory.NonSpacingMark || category == UnicodeCategory.SpacingCombiningMark
            || category == UnicodeCategory.EnclosingMark || next == 0x200D || previous == 0x200D
            || next >= 0xFE00 && next <= 0xFE0F || next >= 0xE0100 && next <= 0xE01EF
            || next >= 0x1F3FB && next <= 0x1F3FF || next >= 0xE0020 && next <= 0xE007F) return false;
        // Older Unity Unicode tables may not combine flags or Hangul jamo.
        if (previous >= 0x1F1E6 && previous <= 0x1F1FF && next >= 0x1F1E6 && next <= 0x1F1FF)
        {
            var count = 0;
            for (var i = index; i >= 2;)
            {
                if (!char.IsLowSurrogate(text[i - 1]) || !char.IsHighSurrogate(text[i - 2])) break;
                var cp = char.ConvertToUtf32(text, i - 2);
                if (cp < 0x1F1E6 || cp > 0x1F1FF) break;
                count++;
                i -= 2;
            }
            if (count % 2 == 1) return false;
        }
        var left = HangulClass(previous);
        var right = HangulClass(next);
        return !(left == 1 && (right == 1 || right == 2 || right == 4 || right == 5)
            || (left == 2 || left == 4) && (right == 2 || right == 3)
            || (left == 3 || left == 5) && right == 3);
    }

    private static int HangulClass(int codepoint)
    {
        if (codepoint >= 0x1100 && codepoint <= 0x115F || codepoint >= 0xA960 && codepoint <= 0xA97C) return 1;
        if (codepoint >= 0x1160 && codepoint <= 0x11A7 || codepoint >= 0xD7B0 && codepoint <= 0xD7C6) return 2;
        if (codepoint >= 0x11A8 && codepoint <= 0x11FF || codepoint >= 0xD7CB && codepoint <= 0xD7FB) return 3;
        if (codepoint >= 0xAC00 && codepoint <= 0xD7A3) return (codepoint - 0xAC00) % 28 == 0 ? 4 : 5;
        return 0;
    }
}
