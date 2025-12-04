import { useEffect, useState } from "react";
import { getCategories, getSubcategoriesByCategory } from "../services/categoryService";
import type { Subcategory } from "../interfaces/Subcategory";
import type { Category } from "../interfaces/Category";
import PageCardLayout from "../layout/PageCardLayout";
import { useTranslation } from "react-i18next";
import CurrencyComponent from "../components/CurrencyComponent";
import MetalComponent from "../components/MetalComponent";


const HomePage = () => {
  const {t, i18n} = useTranslation();

  const [categories, setCategories] = useState<Category[]>([]);
  const [subcategories, setSubcategories] = useState<Subcategory[]>([]);

  const [currencies, setCurrencies] = useState<Subcategory[]>([]);

  const [selectedCategory, setSelectedCategory] = useState<Category | null>(null);
  const [selectedSubcategory, setSelectedSubcategory] = useState<Subcategory | null>(null);


  // Initialize Categories. categories ve selectedCategory set et
  useEffect(() => {
    const fetchCategories = async () => {
      // Kategorileri getir
      const categoriesData = await getCategories();
      setCategories(categoriesData);
      // İlk category verisini selected olarak set et.
      setSelectedCategory(categoriesData.length > 0 ? categoriesData[0] : null);

      // Metals ve Crypto kategorilerinde para birimi dönüşümü için componentlere prop olarak geçmek lazım.
      const getCurrencies = await getSubcategoriesByCategory(1);
      setCurrencies(getCurrencies);
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
      <CurrencyComponent 
       selectedSubcategory={selectedSubcategory}
       subcategories={subcategories} 
       />
    )}
    {selectedCategory?.id === 2 && (
      <MetalComponent
      selectedSubcategory={selectedSubcategory}
      subcategories={subcategories}
      currencies={currencies}
      />
    )}
    {/* Metals Kategorisi seçilmiş ise */}
  </PageCardLayout>
  );
};

export default HomePage;
