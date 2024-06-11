import * as config from "./config";
import { getData} from "../helpers/getData";
var urlEndpoint = config.urlEndpoint;
export const endpoints =
{
    FormBook: {
        GetAll: () => {
            const url = `${urlEndpoint}/Books`;
            const header = { method: 'get' };
            return getData(url, header);
        },
        Add: (formData) => {
            const url = `${urlEndpoint}/Books`;
            const header = { method: 'POST',  body: JSON.stringify(formData), 
                headers: {'Content-Type': 'application/json'} };
            return getData(url, header);
        },  
        Update: (formData) => {
            const url = `${urlEndpoint}/Books/Update?id=${formData.id}`;
            const header = { method: 'PATCH', body: JSON.stringify(formData), 
                headers: {'Content-Type': 'application/json'} };
            return getData(url, header);
        },
        Delete: (id) => {
            const url = `${urlEndpoint}/Books/${id}`;
            const header = { method: 'DELETE'};
            return getData(url, header);
        },
    },
    FormLoans: {
        GetAll: () => {
            const url = `${urlEndpoint}/BookLoan`;
            const header = { method: 'get' };
            return getData(url, header);
        },    
        Add: (formData) => {
            const url = `${urlEndpoint}/BookLoan`;
            const header = { method: 'POST',  body: JSON.stringify(formData), 
                headers: {'Content-Type': 'application/json'} };
            return getData(url, header);
        },  
        Update: (formData) => {
            const url = `${urlEndpoint}/BookLoan/Update?id=${formData.id}`;
            const header = { method: 'PATCH', body: JSON.stringify(formData), 
                headers: {'Content-Type': 'application/json'} };
            return getData(url, header);
        },
        Delete: (id) => {
            const url = `${urlEndpoint}/BookLoan/${id}`;
            const header = { method: 'DELETE'};
            return getData(url, header);
        },
    }
}