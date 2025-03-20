using System;
using System.Diagnostics.Contracts;

namespace CactusLang.Tags;

public class TagContainer {
    private List<Tag> _tags;

    public TagContainer() {
        _tags = new List<Tag>();
    }
}