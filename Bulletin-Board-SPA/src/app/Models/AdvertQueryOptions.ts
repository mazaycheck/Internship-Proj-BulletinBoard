export class AdvertQueryOptions {
    orderBy: string = 'date';
    direction: string = 'desc';
    pageNumber: number;
    pageSize: number;
    category: string = '';
    query: string = '';
    userId: number;
}

