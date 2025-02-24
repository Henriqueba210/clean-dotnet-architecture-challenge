import { test, expect } from '@playwright/test';

test.describe('Location Search Component', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:4200');
  });

  test('should display search form', async ({ page }) => {
    await expect(page.getByTestId('postcode-input')).toBeVisible();
    await expect(page.getByTestId('search-button')).toBeVisible();
  });

  test('should display location details when searching valid postcode', async ({ page }) => {
    // Arrange
    const postcode = 'N7 6RS';
    
    // Act
    await page.getByTestId('postcode-input').fill(postcode);
    await page.getByTestId('search-button').click();

    // Assert
    const currentCard = page.getByTestId('current-location-card');
    await expect(currentCard).toBeVisible();
    await expect(currentCard.getByTestId('card-title')).toHaveText('Current Location');
    await expect(currentCard.getByTestId('distance-summary')).toBeVisible();
  });

  test('should maintain search history', async ({ page }) => {
    // Arrange
    const postcodes = ['N7 6RS', 'SW1A 1AA', 'E1 6AN'];
    
    // Act
    for (const postcode of postcodes) {
      await page.getByTestId('postcode-input').fill(postcode);
      await page.getByTestId('search-button').click();
      await page.waitForTimeout(300); // Wait for animation
    }

    // Assert
    await expect(page.getByTestId('history-section')).toBeVisible();
    
    // Check last search is current
    await expect(page.getByTestId('current-location-card')
      .getByTestId('card-title')).toHaveText('Current Location');
    
    // Check history cards (most recent first)
    for (let i = 0; i < Math.min(postcodes.length - 1, 3); i++) {
      await expect(page.getByTestId(`history-card-${i}`)).toBeVisible();
    }
  });

  test('should show error message for invalid postcode', async ({ page }) => {
    // Arrange
    const invalidPostcode = 'INVALID';

    // Act
    await page.getByTestId('postcode-input').fill(invalidPostcode);
    await page.getByTestId('search-button').click();

    // Assert
    await expect(page.getByText('Location not found')).toBeVisible();
  });
});