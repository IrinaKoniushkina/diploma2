using System;
using System.Collections.Generic;

[Serializable]
public class NodeData
{
    public string id;
    public string title;
    public string category;
    public string description;
    public string h2;
    public string part2;
}

[Serializable]
public class LinkData
{
    public string source;
    public string target;
}

[Serializable]
public class GraphData
{
    public List<NodeData> nodes;
    public List<LinkData> links;
}
