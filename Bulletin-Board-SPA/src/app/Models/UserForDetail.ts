export interface UserForDetail {
    userId: number;
    userName: string;
    phoneNumber: string;
    townId: string;
    townName: string;
    rating: number;
    registrationDate: Date;
    roles?: string[];
    edit?: boolean;
    email?: string;
    isActive?: boolean;
}
