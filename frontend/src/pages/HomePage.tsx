import { useEffect, useState } from "react";
import { getCategories, getSubcategoriesByCategory } from "../services/categoryService";

import type { Subcategory } from "../interfaces/Subcategory";
import type { Category } from "../interfaces/Category";
import PageCardLayout from "../layout/PageCardLayout";
import type { Currency } from "../interfaces/Currency";
import { getPriceHistory } from "../services/priceService";
import type { Price } from "../interfaces/Price";
import PriceChart from "../components/charts/PriceChart";
import { dayOptions } from "./options/DayOptions";
import { useTranslation } from "react-i18next";

const HomePage = () => {
  const {t, i18n} = useTranslation();

  const [loading, setLoading] = useState<boolean>(false);
  const [categories, setCategories] = useState<Category[]>([]);
  const [subcategories, setSubcategories] = useState<Subcategory[]>([]);

  const [selectedCategory, setSelectedCategory] = useState<Category | null>(null);
  const [selectedSubcategory, setSelectedSubcategory] = useState<Subcategory | null>(null);

  const [selectedBaseCurrency, setSelectedBaseCurrency] = useState<Currency | null>(null);
  const [selectedTargetCurrency, setSelectedTargetCurrency] = useState<Currency | null>(null);

  const [priceHistoryDays, setPriceHistoryDays] = useState<number>(7);

  const today = new Date();
  const yesterday = new Date();
  yesterday.setDate(today.getDate() - 1);
  const [priceHistory, setPriceHistory] = useState<Price[]>([]);


  // Initialize Categories. categories ve selectedCategory set et
  useEffect(() => {
    const fetchCategories = async () => {
      // Kategorileri getir
      const categoriesData = await getCategories();
      setCategories(categoriesData);
      // İlk category verisini selected olarak set et.
      setSelectedCategory(categoriesData.length > 0 ? categoriesData[0] : null);
    };
    fetchCategories();
  }, []);

  // SelectedCategory'e ait alt kategorileri getir, Initialize subcategories, selectedSubcategory
  useEffect(() => {
    // Null kontrolü
    if (!selectedCategory) return;
    const fetchSubcategoriesByCategory = async (category_id: number) =>{
      const subcategoriesByCategory = await getSubcategoriesByCategory(category_id);
      setSubcategories(subcategoriesByCategory);
      // Seçili kategorinin ilk alt kategorisini selected yap.
      setSelectedSubcategory(subcategoriesByCategory.length > 0 ? subcategoriesByCategory[0] : null);
    }
    fetchSubcategoriesByCategory(selectedCategory.id);
  }, [selectedCategory]);

  // Currency Kategorisi seçiliyse, Initialize için selectedBaseCurrency selectedSubcategory'e karşılık gelecek, selectedTargetCurrency ise mevcut ilk eleman olacak.
  useEffect(() => {
    if(selectedCategory?.id === 1){
      setSelectedBaseCurrency(selectedSubcategory ? { code: selectedSubcategory.code } : null);
      setSelectedTargetCurrency({ code: subcategories.filter((opt) => opt.code !== selectedSubcategory?.code)[0].code });
    }
  }, [selectedSubcategory]);

  // selectedBaseCurrency || selectedTargetCurrency || priceHistoryDays değiştiğinde tekrar istek gönder
  useEffect(() => {
    const fetchPriceHistory = async () => {
      if(!selectedBaseCurrency || !selectedTargetCurrency) return;
      if(selectedBaseCurrency.code === selectedTargetCurrency.code) return;
      setLoading(true);
      try{
        const response = await getPriceHistory(selectedBaseCurrency.code, selectedTargetCurrency.code, priceHistoryDays);
        setPriceHistory(response);
      } catch(err){
        console.error(`Error while fetching price history for ${selectedBaseCurrency}/${selectedTargetCurrency}: `, err);
      }
      setLoading(false);
    }
    fetchPriceHistory();
  }, [selectedBaseCurrency, selectedTargetCurrency, priceHistoryDays]);

  return (
    <PageCardLayout className="w-full">
    {/* Options Card Layout */}
    <PageCardLayout>
        {/* Category Options */}
        <div className="">
          <div className="h-12  flex flex-wrap justify-center items-center gap-2 bg-surface p-2 rounded-full">
            {categories.map((opt) => (
              <button
                key={opt.id}
                onClick={() => setSelectedCategory(opt)}
                className={`px-4 py-2 rounded-full text-sm font-medium border border-accent transition-colors duration-300
              ${
                selectedCategory?.name === opt.name
                  ? "bg-accent text-white"
                  : "bg-transparent text-text-muted hover:bg-accent/20"
              }
              `}
              >
                {t(`homepage.categories.${opt.name}`)}
              </button>
            ))}
          </div>
          {/* Subcategory Options */}
          <div className="h-12 mt-2 flex flex-wrap justify-center items-center gap-2 bg-surface p-2 rounded-full">
            {subcategories.map((opt) => (
              <button
                key={opt.id}
                onClick={() => setSelectedSubcategory(opt)}
                className={`px-4 py-2 rounded-full text-sm font-medium border border-accent transition-colors duration-300
              ${
                selectedSubcategory === opt
                  ? "bg-accent text-white"
                  : "bg-transparent text-text-muted hover:bg-accent/20"
              }
              `}
              >
                {opt.code}
              </button>
            ))}
          </div>
          <div className="flex justify-center items-center mt-2">
            <label className="text-accent text-xl font-semibold">{t(`homepage.subcategories.${selectedSubcategory?.code}`)}</label>
          </div>
        </div>
    </PageCardLayout>
    {/* Currency Kategorisi seçilmiş ise */}
    {selectedCategory?.id === 1 && (
    <PageCardLayout className="w-3/4">
      {/* Target Currency, PriceHistoryDays */}
      <PageCardLayout>
      <div className="flex justify-between gap-4 my-2 rounded-full py-2 px-4 shadow-sm shadow-accent">
        <label className=" py-2 text-accent font-semibold text-lg">{t("homepage.to")}: </label>
        <select
        className="px-2 py-2 rounded-full border border-accent bg-accent text-white"
        value={selectedTargetCurrency?.code}
        onChange={(e) => setSelectedTargetCurrency({code: e.target.value})}
        >
        {subcategories
          .filter((opt) => opt.code !== selectedBaseCurrency?.code) // Seçili basecurrency, targetcurrency olarak seçilemesin: (USD -> USD) gibi seçimleri engellemek için
          .map((opt) => (
          <>
            <option className=""
            key={opt.code}
            value={opt.code}
            >
              {opt.code}
            </option>
          </>
        ))}
      </select>
      <select
      className="px-2 py-2 rounded-full border border-accent bg-accent text-white"
        value={priceHistoryDays}
        onChange={(e) => setPriceHistoryDays(Number(e.target.value))}
        >
          {dayOptions.map((opt) => (
          <option key={opt.value} value={opt.value}>{t(`homepage.days.${opt.label}`)}</option>
        ))}
      </select>
      </div>
      </PageCardLayout>
      {/* Today, Yesterday Prices */}
      <PageCardLayout className="w-xl">
        <div className="flex flex-row justify-center gap-8">
          <div className="flex flex-col bg-surface rounded-xl shadow-sm shadow-accent p-4 w-2/5 text-accent">
            <h1 className="font-bold mb-2">{t("homepage.yesterday")}</h1>
            <label className="font-semibold">1 {selectedBaseCurrency?.code} = { priceHistory.find((p) => p.date.getDate() === yesterday.getDate())?.rate } {selectedTargetCurrency?.code}</label>
            {/* <label className="font-semibold">{t("homepage.date")}: {priceHistory.find((p) => p.date.getDate() === yesterday.getDate())?.date.toLocaleDateString("tr-TR")}</label> */}
          </div>
          <div className="flex flex-col bg-surface rounded-xl shadow-sm shadow-accent p-4 w-2/5 text-accent">
            <h1 className="font-bold mb-2">{t("homepage.today")}</h1>
            <label className="font-semibold">1 {selectedBaseCurrency?.code} = {priceHistory.find((p) => p.date.getDate() === today.getDate())?.rate} {selectedTargetCurrency?.code}</label>
            {/* <label className="font-semibold">{t("homepage.date")}: {priceHistory.find((p) => p.date.getDate() === today.getDate())?.date.toLocaleDateString("tr-TR")}</label> */}
          </div>
        </div>
      </PageCardLayout>
      {/* Chart */}
      <div className="w-full h-1/4">
        <PriceChart data={priceHistory}/>
      </div>
    </PageCardLayout>
    )}
    {/* Metals Kategorisi seçilmiş ise */}
  </PageCardLayout>
  );
};

export default HomePage;
