using UnityEngine;

namespace SQL_Quest.Components.UI
{
    public class CursorController : MonoBehaviour
    {
        public void ChangeState()
        {
            Cursor.visible = !Cursor.visible;
        }
    }
}
