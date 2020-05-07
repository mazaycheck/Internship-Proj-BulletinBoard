export interface UserForDetail {
    id: number;
    userName: string;
    phoneNumber: string;
    townId: string;
    townName: string;
    rating: number;
    registrationDate: Date;
    roles?: string[];
    edit?: boolean;
    email?: string;
}
