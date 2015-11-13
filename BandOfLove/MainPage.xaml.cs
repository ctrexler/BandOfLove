using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.Band;
using Microsoft.Band.Tiles;
using Microsoft.Band.Tiles.Pages;
using Microsoft.Band.Notifications;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using System.Collections.ObjectModel;
using Windows.ApplicationModel.Background;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace BandOfLove
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        IBandClient bandClient;
        BandTile tile;
        Guid tileGuid;
        public ObservableCollection<Message> Messages { get; set; }

        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;

            Messages = new ObservableCollection<Message>();
            Message m = new Message() { Text = "I love you! :)" };
            Messages.Add(m);
            m = new Message() { Text = "You're so cute! :D" };
            Messages.Add(m);
            m = new Message() { Text = "I miss you" };
            Messages.Add(m);
            m = new Message() { Text = "<3 <3 <3" };
            Messages.Add(m);
            m = new Message() { Text = "HUGGG!!!" };
            Messages.Add(m);
            m = new Message() { Text = "Poke!" };
            Messages.Add(m);

            PairBand();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
        }

        async public void PairBand()
        {
            IBandInfo[] pairedBands = await BandClientManager.Instance.GetBandsAsync();

            try
            {
                bandClient = await BandClientManager.Instance.ConnectAsync(pairedBands[0]);

                // do work after successful connect
                try
                {
                    // do work with firmware & hardware versions
                    connecting.Text = "Connected";
                    check.Text = " ✔";

                    var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
                    if (localSettings.Values.ContainsKey("BOLGuid"))
                    {
                        tileGuid = (Guid)localSettings.Values["BOLGuid"];
                    }
                }
                catch (BandException ex)
                {
                    // handle any BandExceptions
                }
            }
            catch (BandException ex)
            {
                // handle a Band connection exception
            }

            // Subscribe to events
            bandClient.TileManager.TileOpened += EventHandler_TileOpened;
            //bandClient.TileManager.TileClosed += EventHandler_TileClosed;
            //bandClient.TileManager.TileButtonPushed += EventHandler_TileButtonPushed;  
            // Start listening for events
            await bandClient.TileManager.StartReadingsAsync();
        }

        // Define symbolic constants for indexes to each layout that
        // the tile has. The index of the first layout is 0. Because only
        // 5 layouts are allowed, the max index value is 4.
        internal enum TileLayoutIndex
        {
            MessagesLayout = 0,
        };
        // Define symbolic constants to uniquely (in MessagesLayout)
        // identify each of the elements of our layout
        // that contain content that the app will set
        // (that is, these Ids will be used when calling APIs
        // to set the page content).
        internal enum TileMessagesLayoutElementId : short
        {
            Message1 = 1,
            // Id for the 1st message text block
            Message2 = 2,
            // Id for the 2nd message text block
        };

        //async void RegisterBackgroundTasks()
        //{
        //    var access = await BackgroundExecutionManager.RequestAccessAsync();
        //    if (access == BackgroundAccessStatus.Denied)
        //    {
        //        // Handle
        //    }

        //    BackgroundTaskBuilder taskBuilder = new BackgroundTaskBuilder();
        //    taskBuilder.Name = "BackgroundTask";

        //    SystemTrigger trigger = new SystemTrigger(SystemTriggerType.UserAway, false);
        //    taskBuilder.SetTrigger(trigger);
        //    taskBuilder.AddCondition(new SystemCondition(SystemConditionType.UserNotPresent));
        //}

        async private void Click_AddTile(object sender, RoutedEventArgs e)
        {
            try
            {
                // determine the number of available tile slots on the Band
                int tileCapacity = await bandClient.TileManager.GetRemainingTileCapacityAsync();
                if (tileCapacity < 1)
                {
                    SendDialog("You've already", "added this tile :)");
                    return;
                }
            }
            catch (BandException ex)
            {
                // handle a Band connection exception }
            }
            
            // Create the small and tile icons from writable bitmaps.
            // Small icons are 24x24 pixels.
            BandIcon smallIcon;
            StorageFile imageFile_small = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/Band_SmallIcon.png"));
            using (IRandomAccessStream fileStream = await imageFile_small.OpenAsync(FileAccessMode.Read))
            {
                WriteableBitmap bitmap = new WriteableBitmap(24, 24);
                await bitmap.SetSourceAsync(fileStream);
                smallIcon = bitmap.ToBandIcon();
            }

            // Tile icons are 46x46 pixels for Microsoft Band 1, and 48x48 pixels
            // for Microsoft Band 2.
            BandIcon tileIcon;
            StorageFile imageFile_large = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/Band_LargeIcon.png"));
            using (IRandomAccessStream fileStream = await imageFile_large.OpenAsync(FileAccessMode.Read))
            {
                WriteableBitmap bitmap = new WriteableBitmap(48, 48);
                await bitmap.SetSourceAsync(fileStream);
                tileIcon = bitmap.ToBandIcon();
            }

            // create a new Guid for the tile
            Guid newGuid = Guid.NewGuid();
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            localSettings.Values["BOLGuid"] = tileGuid = newGuid;
            // create a new tile with a new Guid
            tile = new BandTile(tileGuid)
            {
                // enable badging (the count of unread messages)
                IsBadgingEnabled = true,
                // set the name
                Name = "Band of Love",
                // set the icons
                SmallIcon = smallIcon,
                TileIcon = tileIcon
            };

            // Create a scrollable vertical panel that will hold 2 text messages.
            ScrollFlowPanel panel = new ScrollFlowPanel
            {
                Rect = new PageRect(0, 0, 245, 102),
                Orientation = FlowPanelOrientation.Vertical,
                ScrollBarColorSource = ElementColorSource.BandBase
            };

            // add the text block to contain the first message
            panel.Elements.Add(
                new WrappedTextBlock
                {
                    ElementId = (short)TileMessagesLayoutElementId.Message1,
                    Rect = new PageRect(0, 0, 245, 102),
                    // left, top, right, bottom margins
                    Margins = new Margins(15, 0, 15, 0),
                    Color = new BandColor(0xFF, 0xFF, 0xFF),
                    Font = WrappedTextBlockFont.Small
                }
            );

            // add the text block to contain the second message
            panel.Elements.Add(
                new WrappedTextBlock
                {
                    ElementId = (short)TileMessagesLayoutElementId.Message2,
                    Rect = new PageRect(0, 0, 245, 102),
                    // left, top, right, bottom margins
                    Margins = new Margins(15, 0, 15, 0),
                    Color = new BandColor(0xFF, 0xFF, 0xFF),
                    Font = WrappedTextBlockFont.Small
                }
            );
            // create the page layout
            PageLayout layout = new PageLayout(panel);

            try
            {     // add the layout to the tile
                tile.PageLayouts.Add(layout);
            }
            catch (BandException ex)
            {
                // handle an error adding the layout
            }

            try
            {
                // add the tile to the Band
                if (await bandClient.TileManager.AddTileAsync(tile))
                {
                    // tile was successfully added
                    // can proceed to set tile content with SetPagesAsync
                }
                else
                {
                    // tile failed to be added, handle error
                }
            }
            catch (BandException ex)
            {
                // handle a Band connection exception }
            }

            // create a new Guid for the messages page
            Guid messagesPageGuid = Guid.NewGuid();
            // create the object that contains the page content to be set
            PageData pageContent = new PageData(
                messagesPageGuid,
                // specify which layout to use for this page
                (int)TileLayoutIndex.MessagesLayout,
                new WrappedTextBlockData(
                    (Int16)TileMessagesLayoutElementId.Message1,
                    "This is the text of the first message"
                ),
                new WrappedTextBlockData(
                    (Int16)TileMessagesLayoutElementId.Message2,
                    "This is the text of the second message"
                )
            );
            try
            {
                // set the page content on the Band
                if (await bandClient.TileManager.SetPagesAsync(tileGuid, pageContent))
                {
                    // page content successfully set on Band
                }
                else
                {
                    // unable to set content to the Band
                }
            }
            catch (BandException ex)
            {
                // handle a Band connection exception
            }
        }

        async private void Click_RemoveAllTiles(object sender, RoutedEventArgs e)
        {
            try
            {
                // get the current set of tiles
                IEnumerable<BandTile> tiles = await bandClient.TileManager.GetTilesAsync();
                foreach (var t in tiles)
                {
                    // remove the tile from the Band
                    if (await bandClient.TileManager.RemoveTileAsync(t))
                    {
                        // do work if the tile was successfully removed
                    }
                }
            }
            catch (BandException ex)
            {
                // handle a Band connection exception
            }
        }

        void EventHandler_TileOpened(object sender, BandTileEventArgs<IBandTileOpenedEvent> e)
        {
            // This method is called when the user taps our Band tile.
            //
            // e.TileEvent.TileId is the tile’s Guid.
            // e.TileEvent.Timestamp is the DateTimeOffset of the event.
            //
            // handle the event
            //SendDialog();
        }

        async private void SendDialog(string title = "Dialog Title", string body = "Dialog body")
        {
            try
            {
                // send a dialog to the Band for one of our tiles
                await bandClient.NotificationManager.ShowDialogAsync(tileGuid, title, body);
            }
            catch (BandException ex)
            {
                // handle a Band connection exception }
            }
        }

        private void Button_SendDialog(object sender, RoutedEventArgs e)
        {
            SendDialog("From Corbin", ((((sender as Button).Parent) as StackPanel).Children.First() as TextBox).Text);
        }

        async private void SendMessage(string title = "Dialog Title", string body = "Dialog body")
        {
            try
            {
                // send a dialog to the Band for one of our tiles
                await bandClient.NotificationManager.SendMessageAsync(tileGuid, title, body, DateTimeOffset.Now, MessageFlags.ShowDialog);
            }
            catch (BandException ex)
            {
                // handle a Band connection exception }
            }
        }

        private void Button_SendMessage(object sender, RoutedEventArgs e)
        {
            SendMessage("From Corbin", ((((sender as Button).Parent) as StackPanel).Children.First() as TextBox).Text);
        }
    }
}
