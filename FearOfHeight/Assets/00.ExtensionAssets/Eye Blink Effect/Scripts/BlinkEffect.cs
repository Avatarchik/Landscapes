using UnityEngine;
using System.Collections;

namespace PostProcess
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
    [AddComponentMenu("Image Effects/Blink Effect")]
    public class BlinkEffect : FOHBehavior
    {
        public Shader standard;
        public Shader curved;

        public static bool isPlaying = false;

        enum State
        {
            FadingIn,
            LongFadingIn,
            FadingOut,
            WaitingForFadeOut,
            Idle
        }

        [Range(0f, 1f)]
        public float smoothness = 0.96f;

        [Range(0f, 1f)]
        public float curvature = 1.0f;

        [Range(0f, 1f)]
        public float time = 0.0f;

        [Range(0f, 10f)]
        public float fadeOutDelay = 0.0f;

        float fadeInTime = 1f;
        float fadeOutTime = 1f;

        public AnimationCurve fadeInCurve;
        public AnimationCurve longFadeInCurve;
        public AnimationCurve fadeOutCurve;

        Material material;
        Material materialCurved;

        float localTime;
        State state;
        bool inAndOut;

        System.Action onFadeInComplete;
        System.Action onFadeOutComplete;

        void Awake()
        {
            base.Awake();
            game.blink = this;
            standard = Shader.Find("Hidden/Image Effects/Blink");
            curved = Shader.Find("Hidden/Image Effects/Blink Curved");

            SetDefaultFadeInAnimationCurves();
            SetDefaultLongFadeInAnimationCurves();
            SetDefaultFadeOutAnimationCurves();
            time = 0f;
            localTime = 0f;
            state = State.Idle;
            inAndOut = true;
            material = new Material(standard);
            materialCurved = new Material(curved);
        }

        void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            Material preferredMaterial = materialCurved;

            if (Mathf.Approximately(curvature, 0f))
                preferredMaterial = material;
            else
                preferredMaterial.SetFloat("_Curvature", curvature * 0.297f);

            float smooth = 80f - smoothness * 75.2f;
            preferredMaterial.SetFloat("_LocalTime", time - 0.3f);
            preferredMaterial.SetFloat("_Smoothness", smooth);
            Graphics.Blit(source, destination, preferredMaterial);
        }

#if UNITY_EDITOR
        float prevLocalTime;
        Camera cameraRef;

        public void RunEditorPreview()
        {
            prevLocalTime = Time.realtimeSinceStartup;
            Blink(null, null);
            UnityEditor.EditorApplication.update -= OnEditorUpdate;
            UnityEditor.EditorApplication.update += OnEditorUpdate;
            cameraRef = GetComponent<Camera>();
        }

        void OnEditorUpdate()
        {
            Update();
            cameraRef.Render();
        }
#endif

        void SetDefaultFadeInAnimationCurves()
        {
            Keyframe begin = new Keyframe();
            begin.time = 0f;
            begin.value = 0f;

            Keyframe end = new Keyframe();
            end.time = 0.0459f;
            end.value = 1f;

            fadeInCurve = new AnimationCurve();
            fadeInCurve.AddKey(begin);
            fadeInCurve.AddKey(end);
            fadeInCurve.postWrapMode = WrapMode.Clamp;
            fadeInCurve.preWrapMode = WrapMode.Clamp;
        }

        void SetDefaultLongFadeInAnimationCurves()
        {
            Keyframe begin = new Keyframe();
            begin.time = 0f;
            begin.value = 0f;

            Keyframe end = new Keyframe();
            end.time = 2f;
            end.value = 1f;

            longFadeInCurve = new AnimationCurve();
            longFadeInCurve.AddKey(begin);
            longFadeInCurve.AddKey(end);
            longFadeInCurve.postWrapMode = WrapMode.Clamp;
            longFadeInCurve.preWrapMode = WrapMode.Clamp;
        }

        void SetDefaultFadeOutAnimationCurves()
        {
            Keyframe begin = new Keyframe();
            begin.time = 0f;
            begin.value = 1f;

            Keyframe end = new Keyframe();
            end.time = 0.829f;
            end.value = 0f;

            fadeOutCurve = new AnimationCurve();
            fadeOutCurve.AddKey(begin);
            fadeOutCurve.AddKey(end);
            fadeOutCurve.postWrapMode = WrapMode.Clamp;
            fadeOutCurve.preWrapMode = WrapMode.Clamp;
        }

        void Update()
        {
            if (state == State.Idle)
                return;

#if UNITY_EDITOR
            if (Application.isPlaying)
                localTime += Time.deltaTime;
            else
            {
                float now = Time.realtimeSinceStartup;
                localTime += now - prevLocalTime;
                prevLocalTime = now;
            }
#else
			localTime += Time.deltaTime;
			
#endif
            if (state == State.FadingIn)
            {
                time = fadeInCurve.Evaluate(localTime);

                if (localTime > fadeInTime)
                {
                    time = 1f;
                    localTime = 0f;
                    if (inAndOut)
                    {
                        if (fadeOutDelay == 0f)
                        {
                            state = State.FadingOut;
                        }
                        else
                            state = State.WaitingForFadeOut;
                    }
                    else
                    {
                        state = State.Idle;
                    }

                    isPlaying = false;
                    if (onFadeInComplete != null)
                    {
                        onFadeInComplete();
                        onFadeInComplete = null;
                    }
                }
            }

            else if (state == State.LongFadingIn)
            {
                time = longFadeInCurve.Evaluate(localTime);

                if (localTime > fadeInTime)
                {
                    time = 1f;
                    localTime = 0f;
                    if (inAndOut)
                    {
                        if (fadeOutDelay == 0f)
                        {
                            state = State.FadingOut;
                        }
                        else
                            state = State.WaitingForFadeOut;
                    }
                    else
                    {
                        state = State.Idle;
                    }

                    isPlaying = false;
                    if (onFadeInComplete != null)
                    {
                        onFadeInComplete();
                        onFadeInComplete = null;
                    }
                }
            }

            else if (state == State.FadingOut)
            {
                time = fadeOutCurve.Evaluate(localTime);

                if (localTime > fadeOutTime)
                {
                    time = 0f;
                    localTime = 0f;
                    state = State.Idle;

#if UNITY_EDITOR
                    UnityEditor.EditorApplication.update -= OnEditorUpdate;
#endif

                    isPlaying = false;
                    if (onFadeOutComplete != null)
                    {
                        onFadeOutComplete();
                        onFadeOutComplete = null;
                    }
                }
            }
            else if (state == State.WaitingForFadeOut)
            {
                if (localTime > fadeOutDelay)
                {
                    localTime = 0f;
                    state = State.FadingOut;
                }
            }
        }

        public void Blink(System.Action onComplete = null, System.Action onFadeInComplete = null)
        {
            inAndOut = true;
            this.onFadeOutComplete = onComplete;
            this.onFadeInComplete = onFadeInComplete;
            time = 0f;
            localTime = 0f;
            fadeInTime = fadeInCurve[fadeInCurve.length - 1].time;
            fadeOutTime = fadeOutCurve[fadeOutCurve.length - 1].time;
            state = State.FadingIn;
            isPlaying = true;
        }

        public void FadeIn(System.Action onComplete = null)
        {
            this.onFadeInComplete = onComplete;
            this.onFadeOutComplete = null;
            state = State.FadingIn;
            inAndOut = false;
            fadeInTime = fadeInCurve[fadeInCurve.length - 1].time;
            time = 0f;
            localTime = 0f;
            isPlaying = true;
        }

        public void LongFadeIn(System.Action onComplete = null)
        {
            this.onFadeInComplete = onComplete;
            this.onFadeOutComplete = null;
            state = State.LongFadingIn;
            inAndOut = false;
            fadeInTime = longFadeInCurve[longFadeInCurve.length - 1].time;
            time = 0f;
            localTime = 0f;
            isPlaying = true;
        }

        public void FadeOut(System.Action onComplete = null)
        {
            this.onFadeInComplete = null;
            this.onFadeOutComplete = onComplete;
            if (fadeOutDelay == 0)
                state = State.FadingOut;
            else
                state = State.WaitingForFadeOut;
            inAndOut = false;
            fadeOutTime = fadeOutCurve[fadeOutCurve.length - 1].time;
            time = 1f;
            localTime = 0f;
            isPlaying = true;
        }
    }
}