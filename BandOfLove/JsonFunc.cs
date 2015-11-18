using Newtonsoft.Json;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Windows.Storage;
using Windows.Storage.Streams;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;
using System.IO;
using Windows.UI;

namespace BandOfLove
{
    class JsonFunc
    {
        //public static string jsonFileName = "BandOfLove.json";

        //public static void AddSweetNothing(string sweetNothing)
        //{
        //    if (!ViewModel.instance.Sets.Any(s => s.Name == set.Name))
        //    {
        //        ViewModel.instance.Sets.Add(set);

        //        Serialize();
        //    }
        //}

        //public static void DeleteSet()
        //{
        //    if (ViewModel.instance.SelectedSet.Decks != null)
        //    {
        //        foreach (Deck d in ViewModel.instance.SelectedSet.Decks.ToList())
        //        {
        //            ViewModel.instance.SelectedDeck = d;
        //            if (ViewModel.instance.SelectedDeck.Cards != null)
        //            {
        //                foreach (Card c in d.Cards.ToList())
        //                {
        //                    ViewModel.instance.SelectedCard = c;
        //                    ViewModel.instance.SelectedDeck.Cards.Remove(ViewModel.instance.SelectedCard);
        //                }
        //            }
        //            ViewModel.instance.SelectedSet.Decks.Remove(ViewModel.instance.SelectedDeck);
        //        }
        //    }
        //    ViewModel.instance.Sets.Remove(ViewModel.instance.SelectedSet);

        //    Serialize();
        //}

        //public static void AddDeck(Deck deck)
        //{
        //    if (!ViewModel.instance.SelectedSet.Decks.Any(d => d.Name == deck.Name))
        //    {
        //        ViewModel.instance.SelectedSet.Decks.Add(deck);

        //        Serialize();
        //    }
        //}

        //async public static void Serialize()
        //{
        //    // Serialize our Product class into a string
        //    // Changed to serialze the List
        //    string jsonContents = JsonConvert.SerializeObject(ViewModel.instance.Sets);

        //    // Get the app data folder and create or replace the file we are storing the JSON in.
        //    StorageFolder localFolder = ApplicationData.Current.LocalFolder;
        //    StorageFile textFile = await localFolder.CreateFileAsync(jsonFileName, CreationCollisionOption.ReplaceExisting);

        //    // Open the file...
        //    using (IRandomAccessStream textStream = await textFile.OpenAsync(FileAccessMode.ReadWrite))
        //    {
        //        // write the JSON string!
        //        using (DataWriter textWriter = new DataWriter(textStream))
        //        {
        //            textWriter.WriteString(jsonContents);
        //            await textWriter.StoreAsync();
        //        }
        //    }
        //}

        //async public static void Deserialize()
        //{
        //    StorageFolder localFolder = ApplicationData.Current.LocalFolder;
        //    StorageFile textFile;
        //    bool FileNotFound = false;
        //    try
        //    {
        //        // Getting JSON from file if it exists, or file not found exception if it does not
        //        textFile = await localFolder.GetFileAsync(jsonFileName);

        //        using (IRandomAccessStream textStream = await textFile.OpenReadAsync())
        //        {
        //            // Read text stream 
        //            using (DataReader textReader = new DataReader(textStream))
        //            {
        //                //get size
        //                uint textLength = (uint)textStream.Size;
        //                await textReader.LoadAsync(textLength);
        //                // read it
        //                string jsonContents = textReader.ReadString(textLength);
        //                // deserialize back to our products!
        //                var sets = JsonConvert.DeserializeObject<ObservableCollection<Set>>(jsonContents);
        //                //await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
        //                //() =>
        //                //{
        //                ViewModel.instance.Sets = sets;
        //                //});
        //            }
        //        }
        //    }
        //    catch (FileNotFoundException ex)
        //    {
        //        FileNotFound = true;
        //    }
        //    if (FileNotFound)
        //    {
        //        textFile = await localFolder.CreateFileAsync(jsonFileName);
        //        ViewModel.instance.Sets.Add(new Set()
        //        {
        //            Name = "Tutorial",
        //            Color = Colors.SteelBlue,
        //            Decks = new ObservableCollection<Deck>()
        //            { }
        //        }
        //        Serialize();
        //    }
        //}
    }
}
