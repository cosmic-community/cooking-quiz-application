// Base Cosmic object interface
export interface CosmicObject {
  id: string;
  slug: string;
  title: string;
  content?: string;
  metadata: Record<string, any>;
  type: string;
  created_at: string;
  modified_at: string;
}

// User object type
export interface User extends CosmicObject {
  type: 'users';
  metadata: {
    email: string;
    username: string;
    role: UserRole;
    total_score?: number;
    quizzes_taken?: number;
    achievements?: string[];
    avatar?: {
      url: string;
      imgix_url: string;
    };
  };
}

// Quiz object type
export interface Quiz extends CosmicObject {
  type: 'quizzes';
  metadata: {
    description?: string;
    category: Category;
    difficulty: DifficultyLevel;
    time_limit?: number; // in minutes
    questions: Question[];
    featured_image?: {
      url: string;
      imgix_url: string;
    };
    total_questions?: number;
    passing_score?: number;
  };
}

// Question object type
export interface Question extends CosmicObject {
  type: 'questions';
  metadata: {
    question_text: string;
    options: QuestionOption[];
    correct_answer: number; // index of correct option
    explanation?: string;
    points?: number;
    category?: Category;
  };
}

// Question option type
export interface QuestionOption {
  id: string;
  text: string;
  is_correct?: boolean;
}

// Result object type
export interface Result extends CosmicObject {
  type: 'results';
  metadata: {
    user: User;
    quiz: Quiz;
    score: number;
    total_questions: number;
    correct_answers: number;
    time_taken?: number; // in seconds
    completed_at: string;
    answers?: AnswerRecord[];
  };
}

// Category object type
export interface Category extends CosmicObject {
  type: 'categories';
  metadata: {
    description?: string;
    color?: string;
    icon?: string;
    parent_category?: Category;
  };
}

// Achievement object type
export interface Achievement extends CosmicObject {
  type: 'achievements';
  metadata: {
    description?: string;
    icon?: string;
    criteria?: string;
    points_required?: number;
    badge_image?: {
      url: string;
      imgix_url: string;
    };
  };
}

// Answer record for storing user answers
export interface AnswerRecord {
  question_id: string;
  selected_option: number;
  is_correct: boolean;
  time_spent?: number;
}

// Enums
export type UserRole = 'user' | 'admin' | 'moderator';
export type DifficultyLevel = 'easy' | 'medium' | 'hard' | 'expert';

// API Response types
export interface CosmicResponse<T> {
  objects: T[];
  total: number;
  limit: number;
  skip: number;
}

// Quiz session state
export interface QuizSession {
  quiz: Quiz;
  currentQuestionIndex: number;
  answers: AnswerRecord[];
  startTime: Date;
  timeRemaining: number;
  isCompleted: boolean;
}

// Leaderboard entry
export interface LeaderboardEntry {
  user: User;
  score: number;
  quizzesTaken: number;
  rank: number;
  achievements: Achievement[];
}

// Type guards
export function isQuiz(obj: CosmicObject): obj is Quiz {
  return obj.type === 'quizzes';
}

export function isQuestion(obj: CosmicObject): obj is Question {
  return obj.type === 'questions';
}

export function isUser(obj: CosmicObject): obj is User {
  return obj.type === 'users';
}

export function isResult(obj: CosmicObject): obj is Result {
  return obj.type === 'results';
}

// Error helper for Cosmic SDK
export function hasStatus(error: unknown): error is { status: number } {
  return typeof error === 'object' && error !== null && 'status' in error;
}