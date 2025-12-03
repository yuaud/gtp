import axios from "axios";
import type { Category } from "../interfaces/Category";
import type { Subcategory } from "../interfaces/Subcategory";

// Category Controller Route
const API_URL = import.meta.env.VITE_API_BASE + "/api/Category";

export const getCategories = async (): Promise<Category[]> => {
    try{
        const response = await axios.get(API_URL);
        return response.data as Category[]; // JSON verisi
    } catch (err) {
        console.error("Error while fetching categories: ", err);
        return [];
    }
};

export const getCategoryById = async (id: number) => {
    try{
        const response = await axios.get(`${API_URL}/${id}`);
        return response.data as Category; // JSON verisi
    } catch (err){
        console.error(`Error while fetching category ${id}: `, err);
        return null;
    }
};

export const getSubcategoriesByCategory = async (category_id: number): Promise<Subcategory[]> => {
    try{
        const response = await axios.get(`${API_URL}/${category_id}/subcategories`);
        const mapped = response.data.map((item: any) => ({
                id: item.id,
                name: item.name,
                code: item.code,
                categoryId: item.categoryId,
                categoryName: item.categoryName
        }))
        return mapped;    // JSON verisi
    } catch (err){
        console.error(`Error while fetching subcategories by category ${category_id}: `, err);
        return [];
    }
};