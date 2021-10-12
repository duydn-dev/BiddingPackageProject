import { environment } from '../../environments/environment';
export class ApiConfig {
    static apiUrl:string = environment.apiUrl;
    static userType:any[] = [
        {status: 0, name: "Không sử dụng"},
        {status: 1, name: "Đang sử dụng"}
    ]
}