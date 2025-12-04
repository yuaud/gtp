import axios from "axios";
import type { Price } from "../interfaces/Price";


const API_URL = import.meta.env.VITE_API_BASE + "/api/Exchange";

export const getLastRate = async(baseCurrency:string, targetCurrency: string): Promise<Price> => {
    try{
        const response = await axios.get(`${API_URL}/${baseCurrency}?to=${targetCurrency}`);
        const mapped: Price = {
            rate: response.data.rate,
            date: new Date(Date.parse(response.data.lastUpdatedUtc))
        };
        return mapped;
    }catch(err){
        console.log("Error while fetching price history: ", err);
        return {
            rate: 0,
            date: new Date(0)
        } as Price;
    }
} 