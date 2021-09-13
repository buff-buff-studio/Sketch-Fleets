using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace SketchFleets
{
    public sealed class MasterController : MonoBehaviour
    {
        [SerializeField]
        private PauseScript pauseScript;
        [FormerlySerializedAs("cyanShoot")]
        [SerializeField]
        private CyanPathDrawer _cyanPathDrawer;

        public void ShootCyan(InputAction.CallbackContext context)
        {
            if (!_cyanPathDrawer.GameHUD.activeSelf) return;
            _cyanPathDrawer.CyanFire(context);
        }

        public void Pause()
        {
            pauseScript.PauseVoid(true);
        }
    }
}