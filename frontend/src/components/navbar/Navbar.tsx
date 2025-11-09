import React, { useContext, useState } from "react";
import { Link } from "react-router";
import { useTheme } from "../../context/ThemeContext";
import { Sun, Moon } from "lucide-react";
import { useTranslation } from "react-i18next";
import { LanguageContext } from "../../context/LanguageContext";

const Navbar = () => {
  const { theme, toggleTheme } = useTheme();
  const { language, setLanguage } = useContext(LanguageContext);
  const { t, i18n } = useTranslation();

  const [languageDropDown, setLanguageDropDown] = useState(false);
  const languages = [
    { code: "en", label: "English", flag: "🇬🇧" },
    { code: "tr", label: "Türkçe", flag: "🇹🇷" },
  ];

  return (
    <nav className="w-full h-14 px-6 py-3 text-text bg-surface flex items-center justify-between">
      {/* Sol Taraf */}
      <div className="flex items-center space-x-6">
        <Link
          className="hover:text-accent font-semibold transition transform duration-300 ease-in-out hover:scale-105"
          to={"/"}
        >
          {t("navbar.home_page")}
        </Link>
        <Link
          className="hover:text-accent font-semibold transition transform duration-300 ease-in-out hover:scale-105"
          to={"/about"}
        >
          {t("navbar.about")}
        </Link>
        <Link
          className="hover:text-accent font-semibold transition transform duration-300 ease-in-out hover:scale-105"
          to={"/contact"}
        >
          {t("navbar.contact")}
        </Link>
      </div>
      {/* Orta */}
      <div>
        <p className="hover:text-accent font-semibold transition transform duration-300 ease-in-out hover:scale-105">
          Orta
        </p>
      </div>
      {/* Sağ Taraf */}
      <div className="flex items-center space-x-4">
        {/* Light/Dark Mode */}
        <button
          className="relative w-18 h-9 rounded-full bg-button-bg flex items-center justify-between px-2 transition-colors duration-300"
          onClick={() => {
            toggleTheme();
          }}
        >
          <Sun className="text-yellow-500" />
          <Moon className="text-white" />
          <span
            className={`absolute top-1 left-1 w-7 h-7 rounded-full bg-white shadow-md transform transition-transform duration-300 ${
              theme === "dark" ? "translate-x-9" : "translate-x-0"
            }`}
          />
        </button>
        {/* Language */}
        <div>
          <button
            onClick={() => setLanguageDropDown(!languageDropDown)}
            className="flex  items-center gap-1.5 px-2 py-1.5 rounded-full bg-button-bg hover:bg-button-hover transition"
          >
            {i18n.language === "tr" ? "🇹🇷" : "🇬🇧"}
            <span className="font-semibold text-white">
              {i18n.language.toUpperCase()}
            </span>
          </button>
          {languageDropDown && (
            <div className="absolute right-2 mt-2  bg-button-bg rounded-lg shadow-lg">
              {languages.map((lng) => (
                <button
                  key={lng.code}
                  onClick={() => {
                    setLanguage(lng.code);
                    setLanguageDropDown(!languageDropDown);
                  }}
                  className="w-full text-left px-4 py-2 hover:bg-button-hover flex items-center gap-2"
                >
                  <span>{lng.flag}</span>
                  <span className="text-white font-semibold">
                    {lng.code.toUpperCase()}
                  </span>
                </button>
              ))}
            </div>
          )}
        </div>
      </div>
    </nav>
  );
};

export default Navbar;
