const fs = require('fs');
const path = require('path');

const scriptTag = '<script src="/dashboard-console-capture.js"></script>';
const htmlFiles = [
  '.next/server/app/index.html',
  '.next/server/app/**/*.html',
  'out/**/*.html'
];

function injectScript(filePath) {
  try {
    let content = fs.readFileSync(filePath, 'utf8');
    if (!content.includes('dashboard-console-capture.js')) {
      content = content.replace('</head>', `  ${scriptTag}\n</head>`);
      fs.writeFileSync(filePath, content);
      console.log(`Injected console capture into ${filePath}`);
    }
  } catch (error) {
    console.error(`Error processing ${filePath}:`, error.message);
  }
}

// Process all HTML files in build output
const glob = require('glob');
htmlFiles.forEach(pattern => {
  glob.sync(pattern).forEach(injectScript);
});