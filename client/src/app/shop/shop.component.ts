import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { IProduct } from '../shared/models/product';
import { ShopService } from './shop.service';
import { IBrand } from '../shared/models/brand';
import { IType } from '../shared/models/type';
import { IPagination } from '../shared/models/pagination';
import { ShopParams } from '../shared/models/shopParams';

@Component({
  selector: 'app-shop',
  templateUrl: './shop.component.html',
  styleUrls: ['./shop.component.scss'],
})
export class ShopComponent implements OnInit {
  @ViewChild('search') searchTerm?: ElementRef; // temporary solution, beforing using Angular Reactive Forms
  products: IProduct[] = [];
  brands: IBrand[] = [];
  types: IType[] = [];
  shopParams: ShopParams = new ShopParams();
  sortOptions = [
    { name: 'Alphabetical', value: 'name' },
    { name: 'Price: Low to High', value: 'priceAsc' },
    { name: 'Price: High to Low', value: 'priceDesc' },
  ];
  totalCount = 0;

  constructor(private shopService: ShopService) {}

  ngOnInit(): void {
    this.getProduts();
    this.getBrands();
    this.getTypes();
  }

  getProduts(): void {
    this.shopService.getProducts(this.shopParams).subscribe({
      next: (response: IPagination<IProduct[]>) => {
        this.products = response.data;
        this.shopParams.pageNumber = response.pageIndex;
        this.shopParams.pageSize = response.pageSize;
        this.totalCount = response.count;
      },
      error: (error) => {
        console.log(error);
      },
      complete: () => {
        console.log('complete');
      },
    });
  }

  getBrands(): void {
    this.shopService.getBrands().subscribe({
      next: (response: IBrand[]) => {
        this.brands = [{ id: 0, name: 'All' }, ...response];
      },
      error: (error) => {
        console.log(error);
      },
    });
  }

  getTypes(): void {
    this.shopService.getTypes().subscribe({
      next: (response: any) => {
        this.types = [{ id: 0, name: 'All' }, ...response];
      },
      error: (error) => {
        console.log(error);
      },
    });
  }

  onBrandSelected(brandId: number): void {
    this.shopParams.brandId = brandId;
    this.getProduts();
  }

  onTypeSelected(typeId: number): void {
    this.shopParams.typeId = typeId;
    this.getProduts();
  }

  onSortSelected(event: Event): void {
    this.shopParams.sort = (event.target as HTMLSelectElement).value;
    this.getProduts();
  }

  onPageChanged(pageNumber: number): void {
    if (this.shopParams.pageNumber !== pageNumber) {
      this.shopParams.pageNumber = pageNumber;
      this.getProduts();
    }
  }

  onSearch(): void {
    this.shopParams.search = this.searchTerm?.nativeElement.value;
    this.getProduts();
  }

  onReset(): void {
    if (this.searchTerm) this.searchTerm.nativeElement.value = '';
    this.shopParams = new ShopParams();
    this.getProduts();
  }
}
