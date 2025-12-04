import React, { useEffect, useState } from 'react'
import type { Subcategory } from '../interfaces/Subcategory';
import type { Currency } from '../interfaces/Currency';
import type { Price } from '../interfaces/Price';
import { useTranslation } from 'react-i18next';
import { getPriceHistory } from '../services/priceService';
import PageCardLayout from '../layout/PageCardLayout';
import { dayOptions } from '../pages/options/DayOptions';
import PriceChart from './charts/PriceChart';
import { ArrowDown, ArrowUp, Equal } from 'lucide-react';

// Props interface
interface CurrencyProps {
    selectedSubcategory: Subcategory | null;
    subcategories: Subcategory[];
}


const CurrencyComponent: React.FC<CurrencyProps> = ({selectedSubcategory, subcategories}) => {
  const {t, i18n} = useTranslation();
  const [loading, setLoading] = useState<boolean>(false);
  const [selectedBaseCurrency, setSelectedBaseCurrency] = useState<Currency | null>(null);
  const [selectedTargetCurrency, setSelectedTargetCurrency] = useState<Currency | null>(null);

  const [priceHistoryDays, setPriceHistoryDays] = useState<number>(7);
  const [todaysPrice, setTodaysPrice] = useState<number>(0);
  const [yesterdaysPrice, setYesterdaysPrice] = useState<number>(0);
  const [change, setChange] = useState<number>(0);

  const today = new Date();
  const yesterday = new Date();
  yesterday.setDate(today.getDate() - 1);
  const [priceHistory, setPriceHistory] = useState<Price[]>([]);

  function formatChange(value: number): string {
  // 6 anlamlı basamağa yuvarla, (en fazla 6 basamak olacak)
  const precise = value.toPrecision(6); 
  // fazla sıfırları kırp
  const trimmed = parseFloat(precise).toString();
  return trimmed;
  }

  // Currency Kategorisi seçiliyse, Initialize için selectedBaseCurrency selectedSubcategory'e karşılık gelecek, selectedTargetCurrency ise mevcut ilk eleman olacak.
  useEffect(() => {
    const target = subcategories.find((opt) => opt.code !== selectedSubcategory?.code);
    setSelectedBaseCurrency(selectedSubcategory ? { code: selectedSubcategory.code } : null);
    target ? setSelectedTargetCurrency({code: target.code}) : setSelectedTargetCurrency(null); 
    //setSelectedTargetCurrency({ code: subcategories.filter((opt) => opt.code !== selectedSubcategory?.code)[0].code });
  }, [selectedSubcategory]);

    // selectedBaseCurrency || selectedTargetCurrency || priceHistoryDays değiştiğinde tekrar istek gönder
    useEffect(() => {
      const fetchPriceHistory = async () => {
        if(selectedSubcategory?.categoryId !== 1) return;
        if(!selectedBaseCurrency || !selectedTargetCurrency) return;
        if(selectedBaseCurrency.code === selectedTargetCurrency?.code) return;
        setLoading(true);
        try{
          const response = await getPriceHistory(selectedBaseCurrency.code, selectedTargetCurrency?.code, priceHistoryDays);
          setPriceHistory(response);
          setYesterdaysPrice(response.find((p) => p.date.getDate() === yesterday.getDate())?.rate ?? 0); 
          setTodaysPrice(response.find((p) => p.date.getDate() === today.getDate())?.rate ?? 0); 
          setChange((response.find((p) => p.date.getDate() === today.getDate())?.rate ?? 0) - (response.find((p) => p.date.getDate() === yesterday.getDate())?.rate ?? 0));
        } catch(err){
          console.error(`Error while fetching price history for ${selectedBaseCurrency}/${selectedTargetCurrency}: `, err);
        }
        setLoading(false);
      }
      fetchPriceHistory();
    }, [selectedBaseCurrency, selectedTargetCurrency, priceHistoryDays]);

  return (
    <div>
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
      <label className="flex justify-center items-center font-semibold text-accent">
        {`${t(`homepage.subcategories.${selectedBaseCurrency?.code}`)} ${t("homepage.to")}: ${t(`homepage.subcategories.${selectedTargetCurrency?.code}`)}`}
      </label>
      </PageCardLayout>
      {/* Today, Yesterday Prices */}
      <PageCardLayout >
        <div className="flex flex-row justify-center gap-8">
          <div className="flex flex-col flex-nowrap bg-surface rounded-xl shadow-sm shadow-accent p-4 min-w-fit text-accent">
            <h1 className="font-bold mb-2">{t("homepage.yesterday")}</h1>
            <label className="font-semibold">1 {selectedBaseCurrency?.code} = { yesterdaysPrice } {selectedTargetCurrency?.code}</label>
            {/* <label className="font-semibold">{t("homepage.date")}: {priceHistory.find((p) => p.date.getDate() === yesterday.getDate())?.date.toLocaleDateString("tr-TR")}</label> */}
          </div>
          <div className="flex flex-col flex-nowrap bg-surface rounded-xl shadow-sm shadow-accent p-4 min-w-fit text-accent">
            <h1 className="font-bold mb-2">{t("homepage.today")}</h1>
            <label className="font-semibold">1 {selectedBaseCurrency?.code} = { todaysPrice } {selectedTargetCurrency?.code}</label>
            {/* <label className="font-semibold">{t("homepage.date")}: {priceHistory.find((p) => p.date.getDate() === today.getDate())?.date.toLocaleDateString("tr-TR")}</label> */}
          </div>
          <div className={`flex flex-row flex-nowrap bg-surface rounded-xl p-4 min-w-fit shadow-sm text-accent ${
              Math.sign(change) === 1 ? "shadow-green-500" : 
              Math.sign(change) === -1 ? "shadow-red-500" : "shadow-gray-500"} ` 
          }>
            <div className="flex flex-col w-2/3 min-w-fit">
              <h1 className="font-bold mb-2">{t("homepage.from_yesterday_to_today")}</h1>
              <label className="font-semibold"> {t("homepage.change")}: {formatChange(change)} </label>
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
      <div className="w-full h-1/4">
        <PriceChart data={priceHistory}/>
      </div>
    </PageCardLayout>
    </div>
  )
}

export default CurrencyComponent;