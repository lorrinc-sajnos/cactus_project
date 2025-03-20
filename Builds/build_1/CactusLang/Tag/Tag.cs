using System;

namespace CactusLang.Tags;

//TODO!!
public class Tag {
    public string Name { get; private set; }

    public Tag(string name) {
        Name = name;
    }
}