using System;

public class AdFinishEventArgs : EventArgs
{
    public string PlacementID { get; private set; }
    public UnityEngine.Advertisements.ShowResult AdShowResult { get; private set; }

    public AdFinishEventArgs(string id,
                             UnityEngine.Advertisements.ShowResult result)
    {
        PlacementID = id;
        AdShowResult = result;
    }
}