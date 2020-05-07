import { Category } from './Category';

export interface Brand {
    brandId: number;
    title: string;
    // categories?: { categoryId: number, title: string }[];
    categories?: string[];
    edit?: boolean;
}
