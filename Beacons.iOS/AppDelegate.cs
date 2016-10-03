﻿using CoreLocation;
using Foundation;
using UIKit;

namespace Beacons.iOS
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the
	// User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
	[Register("AppDelegate")]
	public class AppDelegate : UIApplicationDelegate
	{
		// class-level declarations

		internal static NSUuid RegionUuid => new NSUuid("B9407F30-F5F8-466E-AFF9-25556B57FE6D");

		internal CLLocationManager LocationManager { get; private set; }

		internal CLBeaconRegion BeaconRegion { get; private set; }


		public override UIWindow Window
		{
			get;
			set;
		}

		public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
		{
			// Override point for customization after application launch.
			// If not required for your application you can safely delete this method

			RegisterNotifications();

			LocationManager = new CLLocationManager();
			BeaconRegion = new CLBeaconRegion(RegionUuid,24986, "Community Days 2016");
			BeaconRegion.NotifyEntryStateOnDisplay = true;
			BeaconRegion.NotifyOnEntry = true;
			BeaconRegion.NotifyOnExit = true;

			LocationManager.AuthorizationChanged += (s, e) =>
			{
				if (e.Status != CLAuthorizationStatus.AuthorizedAlways) return;

				LocationManager.RegionEntered += (s1, e1) =>
				{
					SendNotification("Benvenuti alla sessione sui Beacon");
				};
				LocationManager.RegionLeft += (s1, e1) =>
				{
					SendNotification("Non dimenticate di dare il vostro feedback");
				};

				LocationManager.StartMonitoring(BeaconRegion);
			};

			LocationManager.RequestAlwaysAuthorization();
			return true;
		}

		public override void OnResignActivation(UIApplication application)
		{
			// Invoked when the application is about to move from active to inactive state.
			// This can occur for certain types of temporary interruptions (such as an incoming phone call or SMS message) 
			// or when the user quits the application and it begins the transition to the background state.
			// Games should use this method to pause the game.
		}

		public override void DidEnterBackground(UIApplication application)
		{
			// Use this method to release shared resources, save user data, invalidate timers and store the application state.
			// If your application supports background exection this method is called instead of WillTerminate when the user quits.
		}

		public override void WillEnterForeground(UIApplication application)
		{
			// Called as part of the transiton from background to active state.
			// Here you can undo many of the changes made on entering the background.
		}

		public override void OnActivated(UIApplication application)
		{
			// Restart any tasks that were paused (or not yet started) while the application was inactive. 
			// If the application was previously in the background, optionally refresh the user interface.
		}

		public override void WillTerminate(UIApplication application)
		{
			// Called when the application is about to terminate. Save data, if needed. See also DidEnterBackground.
		}

		private static void RegisterNotifications()
		{
			var settings = UIUserNotificationSettings.GetSettingsForTypes(UIUserNotificationType.Alert | UIUserNotificationType.Sound, new NSSet());
			UIApplication.SharedApplication.RegisterUserNotificationSettings(settings);
		}

		private static void SendNotification(string alertBody)
		{
			var notification = new UILocalNotification { AlertBody = alertBody };

			notification.AlertTitle = "Community Days 2016";
			notification.SoundName = UILocalNotification.DefaultSoundName;
			UIApplication.SharedApplication.PresentLocalNotificationNow(notification);
		}
	}
}
