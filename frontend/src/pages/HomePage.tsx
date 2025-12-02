import { useEffect, useState } from "react";
import { getCategories } from "../services/categoryService";
import { getSubcategories } from "../services/subcategoryService";
import type { Subcategory } from "../interfaces/Subcategory";
import type { Category } from "../interfaces/Category";
import PageCardLayout from "../layout/PageCardLayout";
import { LineChart } from "recharts";
import type { Currency } from "../interfaces/Currency";
import { getPriceHistory } from "../services/priceService";
import type { Price } from "../interfaces/Price";
import PriceChart from "../components/charts/PriceChart";

const HomePage = () => {
  const [loading, setLoading] = useState<boolean>(false);

  const [categories, setCategories] = useState<Category[]>([]);
  const [subcategories, setSubcategories] = useState<Subcategory[]>([]);
  const [filteredSubcategories, setFilteredSubcategories] = useState<Subcategory[]>([]);

  const [selectedCategory, setSelectedCategory] = useState<Category | null>(null);
  const [selectedSubcategory, setSelectedSubcategory] = useState<Subcategory | null>(null);

  const [selectedBaseCurrency, setSelectedBaseCurrency] = useState<Currency | null>(null);
  const [selectedTargetCurrency, setSelectedTargetCurrency] = useState<Currency | null>({code: "TRY"});

  const [priceHistoryDays, setPriceHistoryDays] = useState<number>(1);

  const [priceHistory, setPriceHistory] = useState<Price[]>([]);


  useEffect(() => {
    const fetchCategories = async () => {
      // Kategorileri getir
      const categoriesData = await getCategories();
      setCategories(categoriesData);
      // Alt Kategorileri getir
      const subcategoriesData = await getSubcategories();
      setSubcategories(subcategoriesData);
      
      // İlk category verisini selected olarak set et.
      setSelectedCategory(categoriesData.length > 0 ? categoriesData[0] : null);
    };
    fetchCategories();
  }, []);

  useEffect(() => {
    // Null kontrolü
    if (!selectedCategory) {
      setFilteredSubcategories([
        {
        id: 1,
        name: "Alt Kategoriler Getirilemedi",
        code: "NULL",
        categoryId: 1,
        categoryName: "Null"
        }
      ]);
      return;
    }
    // Category seçildiğinde subcategory'i seçili kategoriye göre filtrele
    setFilteredSubcategories(
      subcategories.filter((sub) => sub.categoryId === selectedCategory?.id)
    );
    // Seçili kategorinin ilk alt kategorisini selected yap.
    setSelectedSubcategory(subcategories.find((sub) => sub.categoryId === selectedCategory?.id) ?? null);
  }, [selectedCategory]);

  useEffect(() => {
    // Currency Kategorisi seçiliyse, selectedBaseCurrency selectedSubcategory'e karşılık gelecek.
    if(selectedSubcategory?.categoryId === 1)
      setSelectedBaseCurrency(selectedSubcategory ? { code: selectedSubcategory.code } : null);
  }, [selectedSubcategory]);

  useEffect(() => {
    const fetchPriceHistory = async () => {
      if(!selectedBaseCurrency || !selectedTargetCurrency) return;
      setLoading(true);
      try{
        const response = await getPriceHistory(selectedBaseCurrency.code, selectedTargetCurrency.code, priceHistoryDays);
        console.log("response: ",response);
        
        setPriceHistory(response);
        console.log("istek gonderildi");
        
      } catch(err){
        console.error(`Error while fetching price history for ${selectedBaseCurrency}/${selectedTargetCurrency}: `, err);
      }
      setLoading(false);
    }
    fetchPriceHistory();
    console.log(priceHistory);
    
  }, [selectedBaseCurrency, selectedTargetCurrency, priceHistoryDays]);

  return (
    <PageCardLayout>
    {/* Options Card Layout */}
    <PageCardLayout>
        {/* Category Options */}
        <div className="h-12  flex flex-wrap justify-center items-center gap-2 bg-surface p-2 rounded-full">
          {categories.map((opt) => (
            <button
              key={opt.id}
              onClick={() => setSelectedCategory(opt)}
              className={`px-4 py-2 rounded-full text-sm font-medium transition-colors duration-300
            ${
              selectedCategory?.name === opt.name
                ? "bg-accent text-white"
                : "bg-transparent text-text-muted hover:bg-accent/20"
            }
            `}
            >
              {opt.name}
            </button>
          ))}
        </div>
        {/* Subcategory Options */}
        <div className="h-12 mt-2 flex flex-wrap justify-center items-center gap-2 bg-surface p-2 rounded-full">
          {filteredSubcategories.map((opt) => (
            <button
              key={opt.id}
              onClick={() => setSelectedSubcategory(opt)}
              className={`px-4 py-2 rounded-full text-sm font-medium transition-colors duration-300
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
    </PageCardLayout>
    <PageCardLayout>
      <div style={{ width: 600, height: 600 }}>
        <select
        value={selectedTargetCurrency?.code}
        onChange={(e) => { console.log("selected: ", e.target.value);
         setSelectedTargetCurrency({code: e.target.value});}}
      >
        <option value="USD">USD</option>
        <option value="TRY">TRY</option>
        <option value="EUR">EUR</option>
        <option value="RUB">RUB</option>
        <option value="UAH">UAH</option>
      </select>

      <select
        value={priceHistoryDays}
        onChange={(e) => setPriceHistoryDays(Number(e.target.value))}
      >
        <option value="1">1</option>
        <option value="2">2</option>
        <option value="3">3</option>
        <option value="4">4</option>
        <option value="5">5</option>
      </select>
        <PriceChart data={priceHistory}/>
      </div>
    </PageCardLayout>
  </PageCardLayout>
  );
};

export default HomePage;
