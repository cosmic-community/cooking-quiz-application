import { createBucketClient } from '@cosmicjs/sdk';
import type { 
  Quiz, 
  Question, 
  Result, 
  User, 
  Category, 
  Achievement,
  CosmicResponse
} from '@/types';

// Initialize Cosmic client
export const cosmic = createBucketClient({
  bucketSlug: process.env.COSMIC_BUCKET_SLUG as string,
  readKey: process.env.COSMIC_READ_KEY as string,
  writeKey: process.env.COSMIC_WRITE_KEY as string,
});

// Fetch all quizzes
export async function getQuizzes(category?: string): Promise<Quiz[]> {
  try {
    const query: Record<string, any> = { type: 'quizzes' };
    
    if (category) {
      query['metadata.category.slug'] = category;
    }
    
    const response = await cosmic.objects
      .find(query)
      .props(['id', 'slug', 'title', 'metadata'])
      .depth(2);
    
    return response.objects as Quiz[];
  } catch (error: unknown) {
    if (hasStatus(error) && error.status === 404) {
      return [];
    }
    throw new Error('Failed to fetch quizzes');
  }
}

// Fetch single quiz with questions
export async function getQuiz(slug: string): Promise<Quiz | null> {
  try {
    const response = await cosmic.objects
      .findOne({
        type: 'quizzes',
        slug
      })
      .depth(2);
    
    return response.object as Quiz;
  } catch (error: unknown) {
    if (hasStatus(error) && error.status === 404) {
      return null;
    }
    throw new Error('Failed to fetch quiz');
  }
}

// Fetch categories
export async function getCategories(): Promise<Category[]> {
  try {
    const response = await cosmic.objects
      .find({ type: 'categories' })
      .props(['id', 'slug', 'title', 'metadata'])
      .depth(1);
    
    return response.objects as Category[];
  } catch (error: unknown) {
    if (hasStatus(error) && error.status === 404) {
      return [];
    }
    throw new Error('Failed to fetch categories');
  }
}

// Submit quiz result
export async function submitQuizResult(
  userId: string,
  quizId: string,
  score: number,
  totalQuestions: number,
  correctAnswers: number,
  timeTaken?: number
): Promise<Result> {
  try {
    const response = await cosmic.objects.insertOne({
      type: 'results',
      title: `Result - ${new Date().toISOString()}`,
      metadata: {
        user: userId,
        quiz: quizId,
        score,
        total_questions: totalQuestions,
        correct_answers: correctAnswers,
        time_taken: timeTaken || 0,
        completed_at: new Date().toISOString()
      }
    });
    
    return response.object as Result;
  } catch (error) {
    console.error('Error submitting result:', error);
    throw new Error('Failed to submit quiz result');
  }
}

// Get user's quiz results
export async function getUserResults(userId: string): Promise<Result[]> {
  try {
    const response = await cosmic.objects
      .find({
        type: 'results',
        'metadata.user': userId
      })
      .props(['id', 'metadata'])
      .depth(2);
    
    // Sort by completion date (newest first)
    const results = response.objects as Result[];
    return results.sort((a, b) => {
      const dateA = new Date(a.metadata.completed_at).getTime();
      const dateB = new Date(b.metadata.completed_at).getTime();
      return dateB - dateA;
    });
  } catch (error: unknown) {
    if (hasStatus(error) && error.status === 404) {
      return [];
    }
    throw new Error('Failed to fetch user results');
  }
}

// Get leaderboard data
export async function getLeaderboard(limit: number = 10): Promise<Result[]> {
  try {
    const response = await cosmic.objects
      .find({ type: 'results' })
      .props(['id', 'metadata'])
      .depth(2);
    
    // Sort by score (highest first) and take top results
    const results = response.objects as Result[];
    return results
      .sort((a, b) => b.metadata.score - a.metadata.score)
      .slice(0, limit);
  } catch (error: unknown) {
    if (hasStatus(error) && error.status === 404) {
      return [];
    }
    throw new Error('Failed to fetch leaderboard');
  }
}

// Get user achievements
export async function getUserAchievements(userId: string): Promise<Achievement[]> {
  try {
    const response = await cosmic.objects
      .find({
        type: 'achievements',
        'metadata.users': userId
      })
      .props(['id', 'slug', 'title', 'metadata'])
      .depth(1);
    
    return response.objects as Achievement[];
  } catch (error: unknown) {
    if (hasStatus(error) && error.status === 404) {
      return [];
    }
    throw new Error('Failed to fetch achievements');
  }
}

// Helper function to check if error has status
function hasStatus(error: unknown): error is { status: number } {
  return typeof error === 'object' && error !== null && 'status' in error;
}