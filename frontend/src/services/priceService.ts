import axios from "axios";
import type { Price } from "../interfaces/Price";


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