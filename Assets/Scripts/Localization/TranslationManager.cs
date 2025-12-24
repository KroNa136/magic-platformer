using System.Collections.Generic;
using UnityEngine;

public class TranslationManager : MonoBehaviour
{
    public static TranslationManager Instance;

    public static Dictionary<string, Dictionary<Language, string>> Translations = new()
    {
        {
            "MAGIC",
            new()
            {
                { Language.English, "MAGIC" },
                { Language.Русский, "МАГИЧЕСКИЙ" },
                { Language.Español, "MÁGICO" },
            }
        },
        {
            "PLATFORMER",
            new()
            {
                { Language.English, "PLATFORMER" },
                { Language.Русский, "ПЛАТФОРМЕР" },
                { Language.Español, "PLATAFORMAS" },
            }
        },
        {
            "Continue",
            new()
            {
                { Language.English, "Continue" },
                { Language.Русский, "Продолжить" },
                { Language.Español, "Continuar" },
            }
        },
        {
            "New Game",
            new()
            {
                { Language.English, "New Game" },
                { Language.Русский, "Новая игра" },
                { Language.Español, "Nuevo Juego" },
            }
        },
        {
            "Settings",
            new()
            {
                { Language.English, "Settings" },
                { Language.Русский, "Настройки" },
                { Language.Español, "Ajustes" },
            }
        },
        {
            "Quit",
            new()
            {
                { Language.English, "Quit" },
                { Language.Русский, "Выйти" },
                { Language.Español, "Abandonar" },
            }
        },
        {
            "SETTINGS",
            new()
            {
                { Language.English, "SETTINGS" },
                { Language.Русский, "НАСТРОЙКИ" },
                { Language.Español, "AJUSTES" },
            }
        },
        {
            "Sound",
            new()
            {
                { Language.English, "Sound" },
                { Language.Русский, "Звук" },
                { Language.Español, "Sonido" },
            }
        },
        {
            "Music",
            new()
            {
                { Language.English, "Music" },
                { Language.Русский, "Музыка" },
                { Language.Español, "Música" },
            }
        },
        {
            "Bringer of Death",
            new()
            {
                { Language.English, "Bringer of Death" },
                { Language.Русский, "Вестник Смерти" },
                { Language.Español, "Portador de la Muerte" },
            }
        },
        {
            "STATS",
            new()
            {
                { Language.English, "STATS" },
                { Language.Русский, "АТРИБУТЫ" },
                { Language.Español, "ATRIBUTOS" },
            }
        },
        {
            "Evil Wizardess",
            new()
            {
                { Language.English, "Evil Wizardess" },
                { Language.Русский, "Зловещая Волшебница" },
                { Language.Español, "Hechicera Malvada" },
            }
        },
        {
            "Level",
            new()
            {
                { Language.English, "Level" },
                { Language.Русский, "Уровень" },
                { Language.Español, "Nivel" },
            }
        },
        {
            "Health",
            new()
            {
                { Language.English, "Health" },
                { Language.Русский, "Здоровье" },
                { Language.Español, "Salud" },
            }
        },
        {
            "Mana",
            new()
            {
                { Language.English, "Mana" },
                { Language.Русский, "Мана" },
                { Language.Español, "Maná" },
            }
        },
        {
            "Intellect",
            new()
            {
                { Language.English, "Intellect" },
                { Language.Русский, "Интеллект" },
                { Language.Español, "Intelecto" },
            }
        },
        {
            "PAUSED",
            new()
            {
                { Language.English, "PAUSED" },
                { Language.Русский, "ПАУЗА" },
                { Language.Español, "PAUSA" },
            }
        },
        {
            "Resume",
            new()
            {
                { Language.English, "Resume" },
                { Language.Русский, "Продолжить" },
                { Language.Español, "Reanudar" },
            }
        },
        {
            "To Menu",
            new()
            {
                { Language.English, "To Menu" },
                { Language.Русский, "В Меню" },
                { Language.Español, "Al Menú" },
            }
        },
        {
            "GAME OVER",
            new()
            {
                { Language.English, "GAME OVER" },
                { Language.Русский, "ПОРАЖЕНИЕ" },
                { Language.Español, "HAS PERDIGO" },
            }
        },
        {
            "Respawn",
            new()
            {
                { Language.English, "Respawn" },
                { Language.Русский, "Возродиться" },
                { Language.Español, "Reaparecer" },
            }
        },
        {
            "GAME DONE",
            new()
            {
                { Language.English, "GAME DONE" },
                { Language.Русский, "ПОБЕДА" },
                { Language.Español, "VICTORIA" },
            }
        },
        {
            "Thanks for playing Magic Platformer!",
            new()
            {
                { Language.English, "Thanks for playing Magic Platformer!" },
                { Language.Русский, "Спасибо за игру в Магический Платформер!" },
                { Language.Español, "¡Gracias por jugar Magic Platformer!" },
            }
        },
    };

    public Language CurrentLanguage => (Language) PlayerPrefs.GetInt("language", 0);

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
}
