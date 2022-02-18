using TMPro;
using UnityEngine;
using UnityEngine.Scripting;

namespace Runner.Collection
{
    public class CoinsText : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;

        private ICoinsService _coinsService;

        [Preserve]
        public void Construct(ICoinsService coinsService)
        {
            _coinsService = coinsService;
        }

        private void OnEnable()
        {
            Refresh();
            _coinsService.Changed += Refresh;
        }

        private void OnDisable()
        {
            _coinsService.Changed -= Refresh;
        }

        private void Refresh()
        {
            _text.SetText("{0:0}", _coinsService.Coins);
        }
    }
}