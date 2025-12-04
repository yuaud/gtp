import React, { useEffect, useState } from 'react'
import type { Subcategory } from '../interfaces/Subcategory';
import type { Metal } from '../interfaces/Metal';
import { getPriceMetalHistory } from '../services/priceService';
import type { PriceMetal } from '../interfaces/PriceMetal';
import PageCardLayout from '../layout/PageCardLayout';
import type { Currency } from '../interfaces/Currency';
import { dayOptions } from '../pages/options/DayOptions';
import { useTranslation } from 'react-i18next';
import MetalsChart from './charts/MetalsChart';
import { ArrowDown, ArrowUp, Equal } from 'lucide-react';

// Props interface
interface MetalProps {
    selectedSubcategory: Subcategory | null;
    subcategories: Subcategory[];
    currencies: Subcategory[];
}

const MetalComponent: React.FC<MetalProps> = ({selectedSubcategory, subcategories, currencies}) => {
    const baseCurrency = "USD";
    const {t} = useTranslation();
    const [loading, setLoading] = useState<boolean>(false);
    const [selectedTargetMetal, setSelectedTargetMetal] = useState<Metal | null>(null);
    const [targetCurrency, setTargetCurrency] = useState<Currency | null>({code: "TRY"});

      const [priceHistoryDays, setPriceHistoryDays] = useState<number>(7);
      const [todaysPrice, setTodaysPrice] = useState<number>(0);
      const [yesterdaysPrice, setYesterdaysPrice] = useState<number>(0);
      const [change, setChange] = useState<number>(0);
    
      const today = new Date();
      const yesterday = new Date();
      yesterday.setDate(today.getDate() - 1);
      const [priceHistory, setPriceHistory] = useState<PriceMetal[]>([]);

      useEffect(() => {
        setSelectedTargetMetal(selectedSubcategory ? { code: selectedSubcategory.code } : null);
      }, [selectedSubcategory]);
      useEffect(() => {
        const fetchPriceHistory = async () => {
            if(selectedSubcategory?.categoryId !== 2) return;
            if(!selectedTargetMetal) return;
            setLoading(true);
            try{
                const response = await getPriceMetalHistory(selectedTargetMetal.code, targetCurrency?.code ?? "USD", priceHistoryDays);
                console.log("response: ", response);
                
                setPriceHistory(response);
            } catch (err){
                console.error(`Error while fetching price history for ${selectedTargetMetal}: `, err);
            }
            setLoading(false);
        }
        fetchPriceHistory();
      }, [selectedTargetMetal, targetCurrency, priceHistoryDays]);
  return (
    <div>
        <PageCardLayout className="w-3/4">
            {/* Target Currency, PriceHistoryDays */}
            <PageCardLayout>
            <div className="flex justify-between gap-4 my-2 rounded-full py-2 px-4 shadow-sm shadow-accent">
            <label className=" py-2 text-accent font-semibold text-lg">{t("homepage.to")}: </label>
            <select
                className="px-2 py-2 rounded-full border border-accent bg-accent text-white"
                value={targetCurrency?.code}
                onChange={(e) => setTargetCurrency({code: e.target.value})}
                >
                    {currencies
                    .filter((opt) => opt.code !== baseCurrency)
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
      <PageCardLayout >
        <div className="flex flex-row justify-center gap-8">
          <div className="flex flex-col flex-nowrap bg-surface rounded-xl shadow-sm shadow-accent p-4 min-w-fit text-accent">
            <h1 className="font-bold mb-2">{t("homepage.yesterday")}</h1>
            {/* <label className="font-semibold">1 {selectedBaseCurrency?.code} = { yesterdaysPrice } {selectedTargetCurrency?.code}</label> */}
            {/* <label className="font-semibold">{t("homepage.date")}: {priceHistory.find((p) => p.date.getDate() === yesterday.getDate())?.date.toLocaleDateString("tr-TR")}</label> */}
          </div>
          <div className="flex flex-col flex-nowrap bg-surface rounded-xl shadow-sm shadow-accent p-4 min-w-fit text-accent">
            <h1 className="font-bold mb-2">{t("homepage.today")}</h1>
            {/* <label className="font-semibold">1 {selectedBaseCurrency?.code} = { todaysPrice } {selectedTargetCurrency?.code}</label> */}
            {/* <label className="font-semibold">{t("homepage.date")}: {priceHistory.find((p) => p.date.getDate() === today.getDate())?.date.toLocaleDateString("tr-TR")}</label> */}
          </div>
          <div className={`flex flex-row flex-nowrap bg-surface rounded-xl p-4 min-w-fit shadow-sm text-accent ${
              Math.sign(change) === 1 ? "shadow-green-500" : 
              Math.sign(change) === -1 ? "shadow-red-500" : "shadow-gray-500"} ` 
          }>
            <div className="flex flex-col w-2/3 min-w-fit">
              <h1 className="font-bold mb-2">{t("homepage.from_yesterday_to_today")}</h1>
              {/* <label className="font-semibold"> {t("homepage.change")}: {formatChange(change)} </label> */}
            </div>
            <div className="flex flex-col w-1/3 min-w-fit justify-center items-center ml-4">
              {Math.sign(change) === 1 && ( 
                <> 
                  <ArrowUp className="text-green-500" size={48}/>
                  <label className="font-semibold text-green-500">{t("homepage.price_state.increase")}</label> 
                </>
              )}
              {Math.sign(change) === 0 && (
                <>
                  <Equal className="text-gray-500" size={48}/>
                  <label className="font-semibold text-gray-500">{t("homepage.price_state.same")}</label>
                </>
              )}
              {Math.sign(change) === -1 && ( 
                <>
                  <ArrowDown className="text-red-500" size={48}/>
                  <label className="font-semibold text-red-500">{t("homepage.price_state.decrease")}</label>
                </>
              )}
            </div>
          </div>
        </div>
      </PageCardLayout>
        {/* Chart */}
        <div className="min-w-3xl w-full h-1/4">
            <MetalsChart data={priceHistory}/>
        </div>
        </PageCardLayout>
    </div>
  );
}

export default MetalComponent