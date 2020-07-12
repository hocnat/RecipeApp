# RecipeApp

A Xamarin.Forms recipe app (currently only for Android) that uses Entity Framework Core to access an SQLite database.

[![License](https://img.shields.io/github/license/hocnat/RecipeApp)](https://img.shields.io/github/license/hocnat/RecipeApp)

## Motivation

The idea of this project was to experiment with Xamarin.Forms. At the same time it was a good opportunity to get to know Entity Framework Core better.

## Features

* Add all your favourite recipes

![New recipe overview (German)](Screenshots/RecipeAddOverviewGerman.png)
![New recipe time picker (German)](Screenshots/RecipeAddEditPickTimeGerman.png)

![New recipe ingredients (German)](Screenshots/RecipeAddIngredientsGerman.png)
![New recipe unit picker (German)](Screenshots/RecipeAddEditIngredientsPickUnit.png)

![New recipe directions (German)](Screenshots/RecipeAddDirectionsGerman.png)

* Search for a specific recipe in your list of recipes

![Recipe list with search bar (German)](Screenshots/RecipeListGerman.png)
![Search recipe (German)](Screenshots/SearchRecipeGerman.png)

* View recipe details and let the app calculate ingredients for different numbers of servings

![Recipe details overview (German)](Screenshots/RecipeDetailsOverviewGerman.png)

![Recipe details ingredients servings 2 (German)](Screenshots/RecipeDetailsIngredientsGerman.png)
![Recipe details ingredients servings 1 (German)](Screenshots/RecipeDetailsIngredientsServings1German.png)

![Recipe details directions (German)](Screenshots/RecipeDetailsDirectionsGerman.png)
![Recipe details directions after scrolling (German)](Screenshots/RecipeDetailsDirections2German.png)

* Edit your recipes

![Existing recipe overview (German)](Screenshots/RecipeEditOverviewGerman.png)
![Existing recipe ingredients (German)](Screenshots/RecipeEditIngredientsGerman.png)
![Existing recipe directions (German)](Screenshots/RecipeEditDirectionsGerman.png)

* Delete recipes

![Delete recipe (German)](Screenshots/RecipeDeleteGerman.png)

* Use in English or German (depending on your devices settings)

![New recipe overview (English)](Screenshots/RecipeAddOverviewEnglish.png)
![New recipe overview (German)](Screenshots/RecipeAddOverviewGerman.png)

* Backup and restore your recipes

![Backup and restore recipes (German)](Screenshots/RecipeListBackupRestoreGerman.png)

## Try it yourself

### Clone

Open a command line.

```shell
git clone https://github.com/hocnat/RecipeApp.git
```

### Build

Open Solution in Visual Studio.

Build | Rebuild Solution

### Deploy

> Make sure the Android Emulator is [configured](https://docs.microsoft.com/en-us/xamarin/android/get-started/installation/android-emulator/) correctly or connect an Android device.

Right click on RecipeApp.Android | Set as Startup Project

Build | Deploy Solution

### Start

Open app `Recipes`

## Built with

* [Xamarin.Forms](https://www.nuget.org/packages/Xamarin.Forms/4.7.0.1080) - Microsoft - [MIT License](https://licenses.nuget.org/MIT)
* [Xamarin.Essentials](https://www.nuget.org/packages/Xamarin.Essentials/1.5.3.2) - Microsoft - [MIT License](https://www.nuget.org/packages/Xamarin.Essentials/1.5.3.2/License)
* [Microsoft.EntityFrameworkCore.Sqlite](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Sqlite/3.1.5) - Microsoft - [Apache-2.0 License](https://licenses.nuget.org/Apache-2.0)
* [Xam.Media.Plugin](https://www.nuget.org/packages/Xam.Plugin.Media/5.0.1) - James Montemagno - [MIT License](https://github.com/jamesmontemagno/MediaPlugin/blob/master/LICENSE)
* [Font Awesome](https://fontawesome.com/) - Font Awesome - [SIL OFL 1.1 License](https://scripts.sil.org/cms/scripts/page.php?site_id=nrsi&id=OFL)

## License

[MIT License](https://github.com/hocnat/RecipeApp/blob/master/LICENSE) Copyright 2020 © [hocnat](https://github.com/hocnat)
