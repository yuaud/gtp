import axios from "axios";
import type { Subcategory } from "../interfaces/Subcategory";

// Subcategory Controller Route
const API_URL = import.meta.env.VITE_API_BASE + "/api/Subcategory";

export const getSubcategories = async (): Promise<Subcategory[]> => {
    try{
        const response = await axios.get(API_URL);
        return response.data as Subcategory[]; // JSON verisi
    } catch (err) {
        console.error("Error fetching subcategories: ", err);
        return [];
    }
};

export const getSubcategoryById = async (id: number) => {
    try{
        const response = await axios.get(`${API_URL}/${id}`);
        return response.data as Subcategory; // JSON verisi
    } catch (err){
        console.error(`Error fetching subcategory ${id}: `, err);
        return null;
    }
};