using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class Command : MonoBehaviour
{
    protected abstract void SaveBackup();

    public abstract void Undo();

    public abstract bool Execute();
}
