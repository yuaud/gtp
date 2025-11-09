import { createContext, useEffect, useState } from "react";
import i18n from "../locales/i18n";


export const LanguageContext = createContext({
    language: "tr",
    setLanguage: (lang: string) => {},
});

export function LanguageProvider({children}: {children: React.ReactNode}) {
    const [language, setLanguage] = useState(localStorage.getItem("lang") || "tr");

    useEffect(() => {
        i18n.changeLanguage(language);
        localStorage.setItem("lang", language);
    }, [language]);

    return (
        <LanguageContext.Provider value={{language, setLanguage}}>
            {children}
        </LanguageContext.Provider>
    );
}