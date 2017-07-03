using UnityEngine;

public class MirrorGameObject
{
    public GameObject Me;
    public GameObject Link;

    public MirrorGameObject(GameObject link)
    {
        Link = link;
        Me = new GameObject();

        Me.transform.position = link.transform.position;
        Me.transform.rotation = link.transform.rotation;

        foreach (Transform linkChild in link.transform)
        {
            var child = new MirrorGameObject(linkChild.gameObject);
            child.Me.transform.SetParent(Me.transform);
            child.Me.transform.position = linkChild.position;
            child.Me.transform.rotation = linkChild.rotation;
        }
    }
}
