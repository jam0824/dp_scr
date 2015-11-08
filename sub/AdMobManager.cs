using UnityEngine;
using System.Collections;
using GoogleMobileAds.Api;

public class AdMobManager : MonoBehaviour {
		public string Android_Banner;
		public string Android_Interstitial;
		public string ios_Banner;
		public string ios_Interstitial;

		private InterstitialAd interstitial;
		private AdRequest request;

		bool is_close_interstitial = false; 

		// Use this for initialization
		void Awake () {
				// 起動時にインタースティシャル広告をロードしておく
				RequestInterstitial ();
				// バナー広告を表示
				RequestBanner ();
		}

		// Update is called once per frame
		void Update () {

		}

		public void RequestBanner()
		{
				#if UNITY_ANDROID
				string adUnitId = Android_Banner;
				#elif UNITY_IPHONE
				string adUnitId = ios_Banner;
				#else
				string adUnitId = "unexpected_platform";
				#endif

				// Create a 320x50 banner at the top of the screen.
				BannerView bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);
				// Create an empty ad request.
				AdRequest request = new AdRequest.Builder().Build();
				// Load the banner with the request.
				bannerView.LoadAd(request);
		}

		public void RequestInterstitial()
		{
				#if UNITY_ANDROID
				string adUnitId = Android_Interstitial;
				#elif UNITY_IPHONE
				string adUnitId = ios_Interstitial;
				#else
				string adUnitId = "unexpected_platform";
				#endif

				if (is_close_interstitial == true) {
						interstitial.Destroy ();
				}

				// Initialize an InterstitialAd.
				interstitial = new InterstitialAd (adUnitId);
				// Create an empty ad request.
				request = new AdRequest.Builder ().Build ();
				// Load the interstitial with the request.
				interstitial.LoadAd (request);

				interstitial.AdClosed += HandleAdClosed;

				is_close_interstitial = false;
		}

		// インタースティシャル広告を閉じた時に走る
		void HandleAdClosed (object sender, System.EventArgs e)
		{
				is_close_interstitial = true;
		}
}