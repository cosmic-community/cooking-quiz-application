# Cooking Quiz Application

![App Preview](https://images.unsplash.com/photo-1556909114-f6e7ad7d3136?w=1200&h=300&fit=crop&auto=format,compress)

A comprehensive cooking-themed quiz application built with Next.js, TypeScript, and Cosmic CMS. Test your culinary knowledge across various categories including cuisines, cooking techniques, ingredients, and nutrition while competing on the global leaderboard.

## ✨ Features

- 🎯 **Interactive Quiz Experience**: Engaging multiple-choice questions with immediate feedback
- ⏱️ **Timed Challenges**: 10-minute quiz sessions to test your knowledge under pressure
- 📊 **Smart Scoring System**: Track your performance with detailed score analytics
- 🏆 **Global Leaderboard**: Compete with cooking enthusiasts worldwide
- 🎖️ **Achievement System**: Unlock badges like "Master of Spices 🌶️" and "Baking Pro 🥖"
- 📱 **Responsive Design**: Seamless experience across all devices
- 🔐 **User Authentication**: Secure login and personalized quiz history
- 👨‍💼 **Admin Dashboard**: Comprehensive quiz management for administrators
- 🎨 **Beautiful UI**: Modern, clean design with smooth animations
- 📈 **Progress Tracking**: Monitor your improvement over time

## Clone this Project

## Clone this Project

Want to create your own version of this project with all the content and structure? Clone this Cosmic bucket and code repository to get started instantly:

[![Clone this Project](https://img.shields.io/badge/Clone%20this%20Project-29abe2?style=for-the-badge&logo=cosmic&logoColor=white)](https://app.cosmicjs.com/projects/new?clone_bucket=68bab3cf285c02bfe718dad3&clone_repository=68bab75b285c02bfe718daf3)

## Prompts

This application was built using the following prompts to generate the content structure and code:

### Content Model Prompt

> "🍲 Cooking-Themed Quiz Application (C# + React)
🎯 Core Idea

A web app where users can play cooking-related quizzes, test their knowledge about recipes, cuisines, ingredients, and techniques, while tracking their scores.

🔑 Features
👤 User Features

Play Quiz:

Multiple-choice cooking questions (ingredients, cuisines, nutrition facts).

Timed quizzes (e.g., 10 minutes).

Randomized question sets.

Scoreboard & Progress:

Show user's scores after each quiz.

Track past performance/history.

Categories:

Cuisine-based (Italian, Indian, Japanese).

Cooking techniques (baking, frying, grilling).

Nutrition (calories, vitamins, health benefits).

🔒 Admin Features

Create/Edit/Delete quizzes.

Add questions with multiple choices & correct answers.

Organize quizzes into categories/difficulty levels."

### Code Generation Prompt

> "Build a cooking quiz application using Next.js and TypeScript that works with my Cosmic CMS content model. The app should allow users to take quizzes about cooking, track their scores, view a leaderboard, and earn achievements. Include an admin panel for quiz management."

The app has been tailored to work with your existing Cosmic content structure and includes all the features requested above.

## 🚀 Technologies

- **Frontend Framework**: [Next.js 15](https://nextjs.org/) with App Router
- **Language**: [TypeScript](https://www.typescriptlang.org/) for type-safe development
- **Styling**: [Tailwind CSS](https://tailwindcss.com/) for utility-first styling
- **CMS**: [Cosmic](https://www.cosmicjs.com/docs) for content management
- **Icons**: [Lucide React](https://lucide.dev/) for beautiful icons
- **Authentication**: JWT-based authentication system
- **State Management**: React hooks and context API
- **Deployment**: Optimized for Vercel deployment

## 📋 Getting Started

### Prerequisites

- Node.js 18+ installed
- Bun package manager
- Cosmic account with configured bucket
- Environment variables configured

### Installation

1. Clone the repository:
```bash
git clone [repository-url]
cd cooking-quiz-app
```

2. Install dependencies:
```bash
bun install
```

3. Set up environment variables:
```bash
# Create .env.local file
COSMIC_BUCKET_SLUG=your-bucket-slug
COSMIC_READ_KEY=your-read-key
COSMIC_WRITE_KEY=your-write-key
```

4. Run the development server:
```bash
bun run dev
```

5. Open [http://localhost:3000](http://localhost:3000) in your browser

## 📚 Cosmic SDK Examples

### Fetching Quizzes
```typescript
const quizzes = await cosmic.objects
  .find({ type: 'quizzes' })
  .props(['title', 'slug', 'metadata'])
  .depth(2)
```

### Submitting Quiz Results
```typescript
await cosmic.objects.insertOne({
  type: 'results',
  title: `${userName} - ${quizTitle}`,
  metadata: {
    user: userId,
    quiz: quizId,
    score: finalScore,
    completed_at: new Date().toISOString()
  }
})
```

### Getting Leaderboard
```typescript
const results = await cosmic.objects
  .find({ type: 'results' })
  .props(['metadata'])
  .depth(2)
```

## 🎯 Cosmic CMS Integration

This application integrates seamlessly with Cosmic CMS using the following object types:

- **Users**: User profiles with authentication details and statistics
- **Quizzes**: Quiz configurations with categories and difficulty levels
- **Questions**: Multiple-choice questions with correct answers
- **Results**: Quiz attempt records with scores and timestamps
- **Categories**: Organization structure for quizzes
- **Achievements**: Badges and rewards for user accomplishments

Each content type is fully manageable through the Cosmic dashboard, allowing non-technical users to update quiz content without code changes.

## 🚀 Deployment Options

### Deploy to Vercel (Recommended)
1. Push your code to GitHub
2. Import repository in Vercel
3. Add environment variables in Vercel dashboard
4. Deploy with one click

### Deploy to Netlify
1. Build the project: `bun run build`
2. Deploy the `.next` folder
3. Configure environment variables
4. Set up continuous deployment

### Environment Variables
Configure these in your deployment platform:
- `COSMIC_BUCKET_SLUG`: Your Cosmic bucket identifier
- `COSMIC_READ_KEY`: Read-only API key
- `COSMIC_WRITE_KEY`: Write access API key

---

Built with ❤️ using [Cosmic](https://www.cosmicjs.com) and Next.js

<!-- README_END -->