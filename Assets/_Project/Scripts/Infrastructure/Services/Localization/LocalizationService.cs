using System.Collections;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using YG;

public class LocalizationService : ILocalizationService
{
    private readonly ICoroutineRunner _coroutineRunner;

    public LocalizationService(ICoroutineRunner coroutineRunner)
    {
        _coroutineRunner = coroutineRunner;
    }

    public void SetLanguage()
    {
        try
        {
            _coroutineRunner.StartCoroutine(LoadLocale(YG2.lang));
        }
        catch (System.Exception) { }
    }

    private IEnumerator LoadLocale(string languageIdentifier)
    {
        yield return LocalizationSettings.InitializationOperation;

        LocaleIdentifier localeCode = new LocaleIdentifier(languageIdentifier);

        for (int i = 0; i < LocalizationSettings.AvailableLocales.Locales.Count; i++)
        {
            Locale locale = LocalizationSettings.AvailableLocales.Locales[i];

            if (locale.Identifier == localeCode)
            {
                LocalizationSettings.SelectedLocale = locale;
                yield break;
            }
        }
    }
}