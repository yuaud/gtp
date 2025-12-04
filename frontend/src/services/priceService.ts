import axios from "axios";
import type { Price } from "../interfaces/Price";
import type { PriceMetal } from "../interfaces/PriceMetal";
import { getLastRate } from "./exchangeRateService";

const API_URL = import.meta.env.VITE_API_BASE + "/api/Price";

export const getPriceHistory = async (baseCurrency:string, targetCurrency: string, days: number): Promise<Price[]> => {
    try{
        const response = await axios.get(`${API_URL}/${baseCurrency}/${targetCurrency}?days=${days}`);

        const mapped: Price[] = response.data.map((p: any) => ({
            rate: p.rate,
            date: new Date(Date.parse(p.lastUpdatedUtc))
        }));

        return mapped;
    }catch(err){
        console.log("Error while fetching price history: ", err);
        return [];
    }
}

export const getPriceMetalHistory = async (targetMetal: string, targetCurrency: string, days: number): Promise<PriceMetal[]> => {
    try{
        const response = await axios.get(`${API_URL}/Metals/${targetMetal}?days=${days}`);
        const convertCurrency = await getLastRate("USD", targetCurrency);
        const mapped: PriceMetal[] = response.data.map((p: any) => ({
            metal: p.xMetal * convertCurrency.rate,
            gramMetal: convertOnsToGramWithCurrency(p.usdxMetal, convertCurrency.rate),
            date: new Date(Date.parse(p.lastUpdated))
        }));

        return mapped;
    } catch(err){
        console.log("Error while fetching metals price history: ", err);
        return [];
    }
}

const convertOnsToGramWithCurrency = (onsmetal: any, rate: number): number => {
    const value = (onsmetal * rate) / 31.1034768;
    return Math.trunc(value * 100) / 100;
}