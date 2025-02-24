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

  test('should maintain search history in correct order', async ({ page }) => {
    // Arrange
    const postcodes = ['N7 6RS', 'SW1A 1AA', 'E1 6AN', 'EC1A 1BB'];
    
    // Act & Assert for first search
    await page.getByTestId('postcode-input').fill(postcodes[0]);
    await page.getByTestId('search-button').click();
    await page.waitForTimeout(300); // Wait for animation

    // Verify only current location shown, no history yet
    await expect(page.getByTestId('current-location-card')).toBeVisible();
    await expect(page.getByTestId('current-location-card').getByText(postcodes[0])).toBeVisible();
    await expect(page.getByTestId('history-section')).not.toBeVisible();

    // Perform remaining searches
    for (let i = 1; i < postcodes.length; i++) {
      await page.getByTestId('postcode-input').fill(postcodes[i]);
      await page.getByTestId('search-button').click();
      await page.waitForTimeout(300); // Wait for animation

      // Verify current location
      await expect(page.getByTestId('current-location-card')).toBeVisible();
      await expect(page.getByTestId('current-location-card').getByTestId('card-title'))
        .toHaveText('Current Location');
      await expect(page.getByTestId('current-location-card').getByText(postcodes[i]))
        .toBeVisible();

      // Verify history section
      await expect(page.getByTestId('history-section')).toBeVisible();
      
      // Calculate expected history size (up to 3 most recent, excluding current)
      const historySize = Math.min(i, 3);
      
      // Check history cards (most recent first)
      for (let j = 0; j < historySize; j++) {
        const historyCard = page.getByTestId(`history-card-${j}`);
        await expect(historyCard).toBeVisible();
        // Check postcode in card title specifically
        await expect(historyCard.getByTestId('card-title')).toHaveText(postcodes[i - j - 1]);
      }

      // Verify maximum history size of 3
      await expect(page.getByTestId('history-card-3')).not.toBeVisible();
    }
  });

  test('should toggle history card details', async ({ page }) => {
    // Arrange - perform two searches to have at least one history card
    const postcodes = ['N7 6RS', 'SW1A 1AA'];
    
    for (const postcode of postcodes) {
      await page.getByTestId('postcode-input').fill(postcode);
      await page.getByTestId('search-button').click();
      await page.waitForTimeout(300);
    }

    // Get the first history card's expansion panel
    const historyCard = page.getByTestId('history-card-0');
    const detailsPanel = historyCard.getByTestId('details-panel');

    // Initially details should be hidden
    await expect(detailsPanel.getByText('Basic Information')).not.toBeVisible();

    // Act - click to expand
    await detailsPanel.getByText('View Details').click();
    
    // Assert - details should be visible
    await expect(detailsPanel.getByText('Basic Information')).toBeVisible();
    await expect(detailsPanel.getByText('Geographic Coordinates')).toBeVisible();
    await expect(detailsPanel.getByText(postcodes[0])).toBeVisible();

    // Act - click to collapse
    await detailsPanel.getByText('View Details').click();

    // Assert - details should be hidden again
    await expect(detailsPanel.getByText('Basic Information')).not.toBeVisible();
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