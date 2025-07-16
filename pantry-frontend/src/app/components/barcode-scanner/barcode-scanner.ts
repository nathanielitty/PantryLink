import { Component, OnInit, OnDestroy, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { ZXingScannerModule } from '@zxing/ngx-scanner';

interface ScannedItem {
  barcode: string;
  name: string;
  category: string;
  quantity: number;
  expirationDate: Date;
  timestamp: Date;
}

@Component({
  selector: 'app-barcode-scanner',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule, RouterModule, ZXingScannerModule],
  templateUrl: './barcode-scanner.html',
  styleUrl: './barcode-scanner.scss',
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class BarcodeScanner implements OnInit, OnDestroy {
  pantryId: number = 0;
  pantryName: string = '';
  isLoading: boolean = true;
  
  // Scanner state
  scannerEnabled: boolean = true;
  scannerAvailable: boolean = false;
  hasPermission: boolean = false;
  qrResultString: string = '';
  scanError: string = '';
  cameras: any[] = [];
  
  // Scanner statistics
  scannedCount: number = 0;
  successfulScans: number = 0;
  
  // Item form
  itemForm: FormGroup;
  editItemForm: FormGroup;
  showItemForm: boolean = false;
  showEditItemModal: boolean = false;
  isSaving: boolean = false;
  
  // Scanned items history
  scannedItems: ScannedItem[] = [];
  recentScans: ScannedItem[] = [];
  itemToEdit: ScannedItem | null = null;
  
  // Categories
  categories: string[] = [
    'Canned Goods',
    'Produce',
    'Dairy',
    'Grains',
    'Proteins',
    'Beverages',
    'Snacks',
    'Baking',
    'Condiments',
    'Hygiene',
    'Baby',
    'Other'
  ];
  
  constructor(
    private route: ActivatedRoute,
    private fb: FormBuilder
  ) {
    this.itemForm = this.fb.group({
      barcode: ['', Validators.required],
      name: ['', Validators.required],
      category: ['', Validators.required],
      quantity: [1, [Validators.required, Validators.min(1)]],
      expirationDate: ['', Validators.required],
      brand: [''],
      unit: ['items'],
      isVegan: [false],
      isVegetarian: [false],
      isGlutenFree: [false],
      isOrganic: [false]
    });
    
    this.editItemForm = this.fb.group({
      barcode: ['', Validators.required],
      name: ['', Validators.required],
      category: ['', Validators.required],
      quantity: [1, [Validators.required, Validators.min(1)]],
      expirationDate: ['', Validators.required],
      brand: [''],
      unit: ['items'],
      isVegan: [false],
      isVegetarian: [false],
      isGlutenFree: [false],
      isOrganic: [false]
    });
  }

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.pantryId = +params['pantryId'];
      this.loadPantryDetails();
    });
    
    // Load previously scanned items from local storage
    this.loadScannedItems();
    
    // Initialize recent scans
    this.updateRecentScans();
  }
  
  ngOnDestroy() {
    // Disable scanner when component is destroyed
    this.scannerEnabled = false;
  }

  loadPantryDetails() {
    // Simulate API call
    setTimeout(() => {
      this.pantryName = 'Community Food Bank';
      this.isLoading = false;
    }, 1000);
  }
  
  loadScannedItems() {
    const savedItems = localStorage.getItem(`scanned-items-${this.pantryId}`);
    if (savedItems) {
      try {
        const parsedItems = JSON.parse(savedItems);
        this.scannedItems = parsedItems.map((item: any) => ({
          ...item,
          expirationDate: new Date(item.expirationDate),
          timestamp: new Date(item.timestamp)
        }));
      } catch (error) {
        console.error('Error loading scanned items from local storage', error);
      }
    }
  }
  
  saveScannedItems() {
    try {
      localStorage.setItem(`scanned-items-${this.pantryId}`, JSON.stringify(this.scannedItems));
    } catch (error) {
      console.error('Error saving scanned items to local storage', error);
    }
  }

  onScanSuccess(result: string) {
    this.qrResultString = result;
    this.scannerEnabled = false;
    
    // Pre-fill form with barcode
    this.itemForm.patchValue({
      barcode: result,
      category: this.categories[0]
    });
    
    // Show item form
    this.showItemForm = true;
  }

  onScanError(error: any) {
    console.error('Scanner error:', error);
  }

  onScanFailure(error: any) {
    // This is often triggered when no barcode is detected, so we don't need to log it
  }

  onCamerasFound(devices: MediaDeviceInfo[]) {
    this.scannerAvailable = devices && devices.length > 0;
  }

  onPermissionResponse(granted: boolean) {
    this.hasPermission = granted;
  }

  resetScanner() {
    this.qrResultString = '';
    this.showItemForm = false;
    this.scannerEnabled = true;
    this.itemForm.reset();
  }

  saveItem() {
    if (this.itemForm.valid) {
      this.isSaving = true;
      
      // Prepare new item data
      const newItem: ScannedItem = {
        barcode: this.itemForm.value.barcode,
        name: this.itemForm.value.name,
        category: this.itemForm.value.category,
        quantity: this.itemForm.value.quantity,
        expirationDate: new Date(this.itemForm.value.expirationDate),
        timestamp: new Date()
      };
      
      // Simulate API call
      setTimeout(() => {
        // Add to scanned items history
        this.scannedItems.unshift(newItem);
        
        // Save to local storage
        this.saveScannedItems();
        
        // Reset form and scanner
        this.isSaving = false;
        this.resetScanner();
        
        // Update statistics
        this.scannedCount++;
        this.successfulScans++;
        
        // Update recent scans
        this.updateRecentScans();
      }, 1000);
    }
  }

  clearHistory() {
    this.scannedItems = [];
    this.recentScans = [];
    this.saveScannedItems();
  }

  formatDate(date: Date): string {
    return date.toLocaleDateString();
  }

  formatTime(date: Date): string {
    return date.toLocaleTimeString();
  }
  
  // Methods referenced in the new HTML template
  toggleScanner() {
    this.scannerEnabled = !this.scannerEnabled;
  }
  
  switchCamera() {
    // Implementation for switching camera
    console.log('Switching camera...');
  }
  
  manualEntry() {
    this.showItemForm = true;
    this.scannerEnabled = false;
    this.itemForm.reset();
  }
  
  cancelItemForm() {
    this.showItemForm = false;
    this.scannerEnabled = true;
    this.itemForm.reset();
  }
  
  addItem() {
    this.saveItem();
  }
  
  addItemAndScanNext() {
    this.saveItem();
    this.scannerEnabled = true;
    this.showItemForm = false;
  }
  
  closeEditItemModal() {
    this.showEditItemModal = false;
    this.itemToEdit = null;
    this.editItemForm.reset();
  }
  
  clearRecentScans() {
    this.recentScans = [];
    this.scannedItems = [];
    this.saveScannedItems();
  }
  
  editRecentScan(scan: ScannedItem) {
    this.itemToEdit = scan;
    this.editItemForm.patchValue({
      barcode: scan.barcode,
      name: scan.name,
      category: scan.category,
      quantity: scan.quantity,
      expirationDate: scan.expirationDate
    });
    this.showEditItemModal = true;
  }
  
  removeRecentScan(scan: ScannedItem) {
    const index = this.recentScans.indexOf(scan);
    if (index > -1) {
      this.recentScans.splice(index, 1);
      this.scannedItems.splice(index, 1);
      this.saveScannedItems();
    }
  }
  
  updateRecentScan() {
    if (this.editItemForm.valid && this.itemToEdit) {
      const updatedItem: ScannedItem = {
        ...this.itemToEdit,
        name: this.editItemForm.value.name,
        category: this.editItemForm.value.category,
        quantity: this.editItemForm.value.quantity,
        expirationDate: new Date(this.editItemForm.value.expirationDate)
      };
      
      const index = this.recentScans.indexOf(this.itemToEdit);
      if (index > -1) {
        this.recentScans[index] = updatedItem;
        this.scannedItems[index] = updatedItem;
        this.saveScannedItems();
      }
      
      this.closeEditItemModal();
    }
  }
  
  private updateRecentScans() {
    // Keep only the last 10 scans for display
    this.recentScans = this.scannedItems.slice(-10).reverse();
  }
}
