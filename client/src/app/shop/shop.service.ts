import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { IPagination } from '../shared/models/pagination';
import { IProduct } from '../shared/models/product';
import { Observable } from 'rxjs';
import { IBrand } from '../shared/models/brand';
import { IType } from '../shared/models/type';
// import { ShopModule } from './shop.module';

@Injectable({
  providedIn: 'root' /*ShopModule*/,
})
export class ShopService {
  baseUrl = 'https://localhost:5001/api/';

  constructor(private http: HttpClient) {}

  getProducts(): Observable<IPagination<IProduct[]>> {
    return this.http.get<IPagination<IProduct[]>>(
      this.baseUrl + 'products?pageSize=10'
    );
  }

  getBrands(): Observable<IBrand[]> {
    return this.http.get<IBrand[]>(this.baseUrl + 'products/brands');
  }
  getTypes(): Observable<IType[]> {
    return this.http.get<IType[]>(this.baseUrl + 'products/brands');
  }
}
