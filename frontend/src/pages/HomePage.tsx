import { useEffect, useState } from "react";
import { getCategories } from "../services/categoryService";
import { getSubcategories } from "../services/subcategoryService";
import type { Subcategory } from "../interfaces/Subcategory";
import type { Category } from "../interfaces/Category";

const HomePage = () => {
  const mainOptions = ["Currency", "Metals", "Crypto"];
  const [mainSelected, setMainSelected] = useState<string>("Metals");

  const [categories, setCategories] = useState<Category[]>([]);
  const [subcategories, setSubcategories] = useState<Subcategory[]>([]);

  const subOptions: Record<string, string[]> = {
    Currency: ["Dolar", "Euro", "TL"],
    Metals: ["Altın", "Gümüş", "Platin"],
    Crypto: ["Bitcoin", "Etherium", "Ripple"]
  };

  const [subSelected, setSubSelected] = useState<string>(subOptions[mainSelected][0]);

  useEffect(() => {
    const fetchCategories = async () => {
      const data = await getCategories();
      setCategories(data);
      console.log("data: ", data);
      

      const subdata = await getSubcategories();
      setSubcategories(subdata);
      console.log("subdata: ", subdata);
      
    }
    fetchCategories();
  }, []);

  useEffect(() => {
    setSubSelected(subOptions[mainSelected][0]);
  }, [mainSelected]);

  return (
    <div className="flex w-full min-h-screen bg-bg">
      <p className="text-text">Home Page</p>
      {/* Main Options */}
      <div className="h-12 mt-8 items-center gap-2 bg-surface p-2 rounded-full">
        {categories.map((opt) => (
          <button
            key={opt.id}
            onClick={() => setMainSelected(opt.name)}
            className={`px-4 py-2 rounded-full text-sm font-medium transition-colors duration-300
          ${
            mainSelected === opt.name
              ? "bg-accent text-white"
              : "bg-transparent text-text-muted hover:bg-accent/20"
          }
          `}
          >
            {opt.name}
          </button>
        ))}
      </div>
      {/* Sub Options */}
      <div className="h-12 mt-8 items-center gap-2 bg-surface p-2 rounded-full">
        {subOptions[mainSelected].map((opt) => (
          <button
            key={opt}
            onClick={() => setSubSelected(opt)}
            className={`px-4 py-2 rounded-full text-sm font-medium transition-colors duration-300
          ${
            subSelected === opt
              ? "bg-accent text-white"
              : "bg-transparent text-text-muted hover:bg-accent/20"
          }
          `}
          >
            {opt}
          </button>
        ))}
      </div>
    </div>
  );
};

export default HomePage;
