# PantryLink Frontend

A modern Angular application for connecting communities with food resources through a ZIP-code-centric food pantry network.

## Features

- **ZIP Code Search**: Find nearby food pantries by entering your ZIP code
- **Real-Time Inventory**: See what's available at each pantry with up-to-date information
- **Barcode Scanning**: Quickly add items to pantry inventory using device camera
- **Analytics Dashboard**: Track pantry usage, popular items, and search patterns
- **User Preferences**: Customize experience based on dietary needs and location
- **Responsive Design**: Works seamlessly on desktop, tablet, and mobile devices

## Technology Stack

- **Angular 20**: Modern frontend framework
- **Clarity Design System**: UI component library for a consistent, accessible interface
- **TypeScript**: Type-safe JavaScript for better developer experience
- **RxJS**: Reactive programming for handling asynchronous operations
- **ZXing**: Barcode scanning capabilities

## UI Improvements

The application has been enhanced with Clarity Design System to provide:

1. **Consistent Design Language**: Using Clarity's components and design patterns for a cohesive user experience
2. **Improved Accessibility**: Following best practices for screen readers and keyboard navigation
3. **Responsive Layouts**: Adapting to different screen sizes with grid-based layouts
4. **Modern Visual Style**: Clean, professional appearance with appropriate use of color and typography
5. **Interactive Components**: Intuitive form controls, alerts, cards, and navigation elements

## Key Components

- **Home**: Landing page with ZIP code search and feature highlights
- **Pantry Search**: Find and filter pantries by location and other criteria
- **Pantry Detail**: Comprehensive view of a specific pantry with inventory information
- **Inventory Management**: Add, edit, and remove items from pantry inventory
- **Barcode Scanner**: Scan product barcodes to quickly add items to inventory
- **Analytics Dashboard**: Visual representation of pantry usage and trends
- **User Preferences**: Personalized settings for dietary needs and notifications

## Getting Started

1. Install dependencies:
   ```
   npm install
   ```

2. Start the development server:
   ```
   npm start
   ```

3. Open your browser to `http://localhost:4200`

## Building for Production

```
npm run build
```

This will generate optimized production files in the `dist/` directory.