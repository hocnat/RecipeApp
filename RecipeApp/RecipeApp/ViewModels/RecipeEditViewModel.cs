﻿using Plugin.Media;
using Plugin.Media.Abstractions;
using RecipeApp.Helpers;
using RecipeApp.Models;
using RecipeApp.Resx;
using RecipeApp.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace RecipeApp.ViewModels
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + "}")]
    public class RecipeEditViewModel : BaseViewModel
    {
        public RecipeEditViewModel(IRecipeService recipeService, IAlertService alertService, int? recipeId)
        {
            RecipeService = recipeService;
            AlertService = alertService;
            RecipeId = recipeId;
            SelectImageCommand = new Command(async () => await SelectImage());
            SaveRecipeCommand = new Command(async () => await SaveRecipe());
            CloseCommand = new Command(async () => await Close());
            BackCommand = new Command(async () => await Back());
            ChangeServingsCommand = new Command<ValueChangedEventArgs>(ChangeServings);
            AddIngredientCommand = new Command(AddIngredient);
            DecreaseIngredientOrderCommand = new Command<IngredientEditViewModel>(DecreaseIngredientOrder);
            IncreaseIngredientOrderCommand = new Command<IngredientEditViewModel>(IncreaseIngredientOrder);
            DeleteIngredientCommand = new Command<IngredientEditViewModel>(DeleteIngredient);
            AddDirectionCommand = new Command(AddDirection);
            DecreaseDirectionOrderCommand = new Command<DirectionEditViewModel>(DecreaseDirectionOrder);
            IncreaseDirectionOrderCommand = new Command<DirectionEditViewModel>(IncreaseDirectionOrder);
            DeleteDirectionCommand = new Command<DirectionEditViewModel>(DeleteDirection);
        }

        public Recipe Recipe
        {
            get
            {
                return recipe;
            }
            set
            {
                if (recipe != value)
                {
                    recipe = value;

                    OnPropertyChanged(nameof(Recipe));
                    OnPropertyChanged(nameof(ImageSource));

                    IngredientEditViewModels = new ObservableCollection<IngredientEditViewModel>(Recipe.Ingredients
                        .OrderBy(ingredient => ingredient.Order)
                        .Select(ingredient => new IngredientEditViewModel(ingredient)));

                    DirectionEditViewModels = new ObservableCollection<DirectionEditViewModel>(Recipe.Directions
                        .OrderBy(direction => direction.Order)
                        .Select(direction => new DirectionEditViewModel(direction)));
                }
            }
        }

        private Recipe recipe;

        public ImageSource ImageSource => ImageHelper.GetImageSource(Recipe?.ImagePath);

        public ObservableCollection<IngredientEditViewModel> IngredientEditViewModels
        {
            get
            {
                return ingredientEditViewModels;
            }
            set
            {
                if (ingredientEditViewModels != value)
                {
                    ingredientEditViewModels = value;
                    RaisePropertyChanged();
                }
            }
        }

        private ObservableCollection<IngredientEditViewModel> ingredientEditViewModels;

        public ObservableCollection<DirectionEditViewModel> DirectionEditViewModels
        {
            get
            {
                return directionEditViewModels;
            }
            set
            {
                if (directionEditViewModels != value)
                {
                    directionEditViewModels = value;
                    RaisePropertyChanged();
                }
            }
        }

        private ObservableCollection<DirectionEditViewModel> directionEditViewModels;

        private IRecipeService RecipeService { get; set; }

        private IAlertService AlertService { get; set; }

        private int? RecipeId { get; set; }

        public async Task Load()
        {
            Recipe = RecipeId == null ? new Recipe() : await LoadRecipe((int)RecipeId);
            Recipe.PropertyChanged += Recipe_PropertyChanged;
        }

        private Task<Recipe> LoadRecipe(int recipeId)
        {
            return Task.Run(() =>
            {
                return RecipeService.GetRecipeAsync(recipeId);
            });
        }

        private void Recipe_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Recipe.ImagePath):
                    OnPropertyChanged(nameof(ImageSource));
                    break;
            }
        }

        public ICommand SelectImageCommand { get; private set; }

        private async Task SelectImage()
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await AlertService.DisplayErrorAlert(nameof(AppResources.PickPhotoNotSupported));

                return;
            }

            var mediaOptions = new PickMediaOptions
            {
                PhotoSize = PhotoSize.MaxWidthHeight,
                MaxWidthHeight = 1080
            };

            var selectedImageFile = await CrossMedia.Current.PickPhotoAsync(mediaOptions);

            if (selectedImageFile == null)
            {
                await AlertService.DisplayErrorAlert(nameof(AppResources.NoPhotoPicked));

                return;
            }

            Recipe.ImagePath = ImageHelper.CopyImage(selectedImageFile.Path);
        }

        public ICommand SaveRecipeCommand { get; private set; }

        private async Task SaveRecipe()
        {
            await RecipeService.SaveRecipeAsync(Recipe);

            await Navigation.PopModalAsync();
        }

        public ICommand CloseCommand { get; private set; }

        private async Task Close()
        {
            await SuggestSaveAndReturnToPreviousPage();
        }

        public ICommand BackCommand { get; private set; }

        private async Task Back()
        {
            await SuggestSaveAndReturnToPreviousPage();
        }

        private async Task SuggestSaveAndReturnToPreviousPage()
        {
            var saveChanges = await AlertService.DisplayQuestionAlert(nameof(AppResources.QuestionSaveChanges));

            if (saveChanges)
            {
                await SaveRecipe();
            }

            await Navigation.PopModalAsync();
        }

        public ICommand ChangeServingsCommand { get; private set; }

        private void ChangeServings(ValueChangedEventArgs e)
        {
            if (e.OldValue == 0 || e.NewValue == 0 || IngredientEditViewModels == null)
                return;

            foreach (var ingredientEditViewModel in IngredientEditViewModels)
            {
                var oldValue = Convert.ToDecimal(e.OldValue);
                var newValue = Convert.ToDecimal(e.NewValue);
                ingredientEditViewModel.Ingredient.Quantity = ingredientEditViewModel.Ingredient.Quantity / oldValue * newValue;
            }
        }

        public ICommand AddIngredientCommand { get; private set; }

        private void AddIngredient()
        {
            var ingredient = Recipe.CreateIngredient();

            IngredientEditViewModels.Add(new IngredientEditViewModel(ingredient));
        }

        public ICommand DecreaseIngredientOrderCommand { get; private set; }

        private void DecreaseIngredientOrder(IngredientEditViewModel ingredientEditViewModel)
        {
            if (Recipe.DecreaseOrder(ingredientEditViewModel.Ingredient))
            {
                var index = IngredientEditViewModels.IndexOf(ingredientEditViewModel);
                IngredientEditViewModels.Move(index, index - 1);
            }
        }

        public ICommand IncreaseIngredientOrderCommand { get; private set; }

        private void IncreaseIngredientOrder(IngredientEditViewModel ingredientEditViewModel)
        {
            if (Recipe.IncreaseOrder(ingredientEditViewModel.Ingredient))
            {
                var index = IngredientEditViewModels.IndexOf(ingredientEditViewModel);
                IngredientEditViewModels.Move(index, index + 1);
            }
        }

        public ICommand DeleteIngredientCommand { get; private set; }

        private void DeleteIngredient(IngredientEditViewModel ingredientEditViewModel)
        {
            Recipe.Remove(ingredientEditViewModel.Ingredient);

            IngredientEditViewModels.Remove(ingredientEditViewModel);
        }

        public ICommand AddDirectionCommand { get; private set; }

        private void AddDirection()
        {
            var direction = Recipe.CreateDirection();

            DirectionEditViewModels.Add(new DirectionEditViewModel(direction));
        }

        public ICommand DecreaseDirectionOrderCommand { get; private set; }

        private void DecreaseDirectionOrder(DirectionEditViewModel directionEditViewModel)
        {
            if (Recipe.DecreaseOrder(directionEditViewModel.Direction))
            {
                var index = DirectionEditViewModels.IndexOf(directionEditViewModel);
                DirectionEditViewModels.Move(index, index - 1);
            }
        }

        public ICommand IncreaseDirectionOrderCommand { get; private set; }

        private void IncreaseDirectionOrder(DirectionEditViewModel directionEditViewModel)
        {
            if (Recipe.IncreaseOrder(directionEditViewModel.Direction))
            {
                var index = DirectionEditViewModels.IndexOf(directionEditViewModel);
                DirectionEditViewModels.Move(index, index + 1);
            }
        }

        public ICommand DeleteDirectionCommand { get; private set; }

        private void DeleteDirection(DirectionEditViewModel directionEditViewModel)
        {
            Recipe.Remove(directionEditViewModel.Direction);

            DirectionEditViewModels.Remove(directionEditViewModel);
        }

        private string DebuggerDisplay => Recipe?.Name;
    }
}