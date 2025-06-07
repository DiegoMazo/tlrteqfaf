using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace CromisDev.CardMatchingSystem
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class ComboTextAnimator : MonoBehaviour
    {
        [SerializeField] private AnimationCurve scaleCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        [SerializeField] private float animationDuration = 0.6f;
        [SerializeField] private float holdDuration = 0.5f;
        [SerializeField] private Vector3 baseScale = Vector3.zero;
        [SerializeField] private Vector3 peakScale = Vector3.one * 2f;

        private TextMeshProUGUI label;
        private CancellationTokenSource animationCts;

        private void Awake()
        {
            label = GetComponent<TextMeshProUGUI>();
            label.text = "";
            label.enabled = false;
        }

        private void OnEnable()
        {
            ComboTracker.OnComboIncreased += ShowCombo;
        }

        private void OnDisable()
        {
            ComboTracker.OnComboIncreased -= ShowCombo;
            animationCts?.Cancel();
        }

        private void ShowCombo(uint count)
        {
            if (count > 1)
                _ = PlayComboAnimationAsync(count);
        }

        private async Task PlayComboAnimationAsync(uint count)
        {
            CancelOngoingAnimation();

            animationCts = new CancellationTokenSource();
            var token = animationCts.Token;

            label.text = $"Combo {count}!";
            label.enabled = true;

            await AnimateScale(baseScale, peakScale, animationDuration, token);
            await DelaySafe(holdDuration, token);
            await AnimateScale(peakScale, baseScale, animationDuration * 0.5f, token);

            label.enabled = false;
        }

        private void CancelOngoingAnimation()
        {
            if (animationCts != null)
            {
                animationCts.Cancel();
                animationCts.Dispose();
                animationCts = null;
            }
        }

        private async Task AnimateScale(Vector3 from, Vector3 to, float duration, CancellationToken token)
        {
            float time = 0f;

            while (time < duration)
            {
                if (token.IsCancellationRequested) return;

                float t = time / duration;
                float curveValue = scaleCurve.Evaluate(t);
                label.transform.localScale = Vector3.LerpUnclamped(from, to, curveValue);

                time += Time.deltaTime;
                await Task.Yield();
            }

            label.transform.localScale = to;
        }

        private static async Task DelaySafe(float delaySeconds, CancellationToken token)
        {
            try
            {
                await Task.Delay((int)(delaySeconds * 1000), token);
            }
            catch (TaskCanceledException) { }
        }
    }
}
