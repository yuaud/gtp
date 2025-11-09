import i18n, { loadResources } from "i18next";
import { initReactI18next } from "react-i18next";
import en from "./en/translation.json";
import tr from "./tr/translation.json";

i18n.use(initReactI18next).init({
  resources: {
    en: { translation: en },
    tr: { translation: tr },
  },
  lng: "tr",
  fallbackLng: "en",
  interpolation: { escapeValue: false },
});

export default i18n;
