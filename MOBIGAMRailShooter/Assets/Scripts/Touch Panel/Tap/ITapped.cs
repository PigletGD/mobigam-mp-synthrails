using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITapped
{
    void OnTap(object sender, TapEventArgs args);
}
