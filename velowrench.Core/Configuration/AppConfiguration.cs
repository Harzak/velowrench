using Avalonia;
using Avalonia.Styling;
using Avalonia.Threading;
using System.Globalization;
using System.Reflection;
using UnitsNet.Units;
using velowrench.Calculations.Interfaces;
using velowrench.Core.Interfaces;
using velowrench.Repository.Interfaces;
using velowrench.Repository.Models;
using velowrench.Utils.Results;

namespace velowrench.Core.Configuration;

/// <summary>
/// Provides application-wide configuration settings.
/// </summary>
public sealed class AppConfiguration : IAppConfiguration
{
    private readonly IUserPreferenceRepository _userPreferenceRepository;
    private readonly IUnitStore _unitStore;

    private CultureInfo _currentCulture;
    private ThemeVariant _currentTheme;
    private LengthUnit _preferredLengthUnit;
    private LengthUnit _preferredDistanceUnit;
    private SpeedUnit _preferredSpeedUnit;

    ///<inheritdoc/>    
    public IReadOnlyCollection<CultureInfo> SupportedCultures { get; }
    ///<inheritdoc/>    
    public CultureInfo CurrentCulture
    {
        get => _currentCulture;
        set
        {
            if (!_currentCulture.Equals(value))
            {
                if (!this.SupportedCultures.Contains(value))
                {
                    throw new InvalidOperationException("Unsupported culture");
                }
                _currentCulture = value;
                CultureInfo.DefaultThreadCurrentCulture = value;
            }
        }
    }

    ///<inheritdoc/>    
    public IReadOnlyCollection<ThemeVariant> AvailableThemes { get; }
    ///<inheritdoc/>    
    public ThemeVariant CurrentTheme
    {
        get => _currentTheme;
        set
        {
            if (_currentTheme != value)
            {
                if (!this.AvailableThemes.Contains(value))
                {
                    throw new InvalidOperationException("Unsupported theme");
                }
                _currentTheme = value;
                Dispatcher.UIThread.Invoke(() =>
                {
                    if (Application.Current != null && Application.Current.RequestedThemeVariant != value)
                    {
                        Application.Current.RequestedThemeVariant = value;
                    }
                });
            }
        }
    }

    ///<inheritdoc/>    
    public LengthUnit PreferredLengthUnit
    {
        get => _preferredLengthUnit;
        set
        {
            if (_preferredLengthUnit != value)
            {
                _preferredLengthUnit = value;
                _unitStore.LengthDefaultUnit = value;
            }
        }
    }
    ///<inheritdoc/>    
    public LengthUnit PreferredDistanceUnit
    {
        get => _preferredDistanceUnit;
        set
        {
            if (_preferredDistanceUnit != value)
            {
                _preferredDistanceUnit = value;
                _unitStore.DistanceDefaultUnit = value;
            }
        }
    }
    ///<inheritdoc/>    
    public SpeedUnit PreferredSpeedUnit
    {
        get => _preferredSpeedUnit;
        set
        {
            if (_preferredSpeedUnit != value)
            {
                _preferredSpeedUnit = value;
                _unitStore.SpeedDefaultUnit = value;
            }
        }
    }

    public Version AppVersion { get; }

    public AppConfiguration(
    IUserPreferenceRepository userPreferenceRepository,
    IUnitStore unitStore)
    {
        _userPreferenceRepository = userPreferenceRepository ?? throw new ArgumentNullException(nameof(userPreferenceRepository));
        _unitStore = unitStore ?? throw new ArgumentNullException(nameof(unitStore));
        _currentCulture = CultureInfo.InvariantCulture;

        this.SupportedCultures = [new("en-US"), new("fr-FR")];
        this.AvailableThemes = [ThemeVariant.Light, ThemeVariant.Dark, ThemeVariant.Default];
        this.AppVersion = Assembly.GetEntryAssembly()?.GetName()?.Version ?? new Version(0, 0, 0, 0);
    }

    public async Task InitializeAsync()
    {
        if (!await TryLoadSavedPreferencesAsync().ConfigureAwait(false))
        {
            LoadDefaultPreferences();
        }
    }

    private async Task<bool> TryLoadSavedPreferencesAsync()
    {
        OperationResult<UserPreferenceModel> preferencesResult = await _userPreferenceRepository.LoadAsync().ConfigureAwait(false);
        if (preferencesResult.IsSuccess && preferencesResult.HasContent)
        {
            this.CurrentCulture = this.SupportedCultures.FirstOrDefault(c => c.Name.Equals(preferencesResult.Content.AppLanguage, StringComparison.OrdinalIgnoreCase))
                                        ?? this.GetDefaultCulture();

            this.CurrentTheme = this.AvailableThemes.FirstOrDefault(x => (string)x.Key == preferencesResult.Content.Theme)
                                      ?? this.GetDefaultTheme();

            this.PreferredLengthUnit = Enum.TryParse<LengthUnit>(preferencesResult.Content.LengthUnit, out LengthUnit lengthUnit)
                                           ? lengthUnit : _unitStore.LengthDefaultUnit;

            this.PreferredDistanceUnit = Enum.TryParse<LengthUnit>(preferencesResult.Content.DistanceUnit, out LengthUnit distanceUnit)
                                            ? distanceUnit : _unitStore.DistanceDefaultUnit;

            this.PreferredSpeedUnit = Enum.TryParse<SpeedUnit>(preferencesResult.Content.SpeedUnit, out SpeedUnit speedUnit)
                                          ? speedUnit : _unitStore.SpeedDefaultUnit;
            return true;
        }
        return false;
    }

    private void LoadDefaultPreferences()
    {
        this.CurrentCulture = this.GetDefaultCulture();
        this.CurrentTheme = this.GetDefaultTheme();
        this.PreferredLengthUnit =  _unitStore.LengthDefaultUnit;
        this.PreferredDistanceUnit = _unitStore.DistanceDefaultUnit;
        this.PreferredSpeedUnit = _unitStore.SpeedDefaultUnit;
    }

    public async Task SaveAsync()
    {
        UserPreferenceModel preferences = new()
        {
            AppLanguage = this.CurrentCulture.Name,
            Theme = (string)this.CurrentTheme.Key,
            LengthUnit = this.PreferredLengthUnit.ToString(),
            DistanceUnit = this.PreferredDistanceUnit.ToString(),
            SpeedUnit = this.PreferredSpeedUnit.ToString()
        };
        await _userPreferenceRepository.SaveAsync(preferences).ConfigureAwait(false);
    }

    private CultureInfo GetDefaultCulture()
    {
        return this.SupportedCultures.First();
    }

    private ThemeVariant GetDefaultTheme()
    {
        return Application.Current?.RequestedThemeVariant ?? ThemeVariant.Default;
    }

}
