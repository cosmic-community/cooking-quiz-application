import Link from 'next/link'
import { Clock, BookOpen, Target } from 'lucide-react'
import type { Quiz } from '@/types'

interface QuizCardProps {
  quiz: Quiz
}

export default function QuizCard({ quiz }: QuizCardProps) {
  const imageUrl = quiz.metadata?.featured_image?.imgix_url
  const category = quiz.metadata?.category
  const difficulty = quiz.metadata?.difficulty || 'medium'
  const timeLimit = quiz.metadata?.time_limit || 10
  const totalQuestions = quiz.metadata?.questions?.length || 0

  const getDifficultyColor = (level: string) => {
    switch (level) {
      case 'easy':
        return 'bg-success text-white'
      case 'medium':
        return 'bg-warning text-white'
      case 'hard':
        return 'bg-error text-white'
      case 'expert':
        return 'bg-secondary text-white'
      default:
        return 'bg-gray-500 text-white'
    }
  }

  return (
    <Link href={`/quizzes/${quiz.slug}`}>
      <div className="quiz-card h-full flex flex-col">
        {imageUrl && (
          <div className="relative h-48 mb-4 -m-6 mb-4">
            <img
              src={`${imageUrl}?w=800&h=400&fit=crop&auto=format,compress`}
              alt={quiz.title}
              className="w-full h-full object-cover rounded-t-lg"
            />
            <div className="absolute top-2 right-2">
              <span className={`px-3 py-1 rounded-full text-xs font-semibold ${getDifficultyColor(difficulty)}`}>
                {difficulty.charAt(0).toUpperCase() + difficulty.slice(1)}
              </span>
            </div>
          </div>
        )}
        
        <h3 className="text-xl font-semibold mb-2">{quiz.title}</h3>
        
        {category && (
          <p className="text-primary font-medium mb-2">
            {category.title}
          </p>
        )}
        
        {quiz.metadata?.description && (
          <p className="text-gray-600 mb-4 line-clamp-2 flex-grow">
            {quiz.metadata.description}
          </p>
        )}
        
        <div className="flex items-center gap-4 text-sm text-gray-500 mt-auto">
          <span className="flex items-center gap-1">
            <BookOpen className="w-4 h-4" />
            {totalQuestions} Questions
          </span>
          <span className="flex items-center gap-1">
            <Clock className="w-4 h-4" />
            {timeLimit} min
          </span>
          <span className="flex items-center gap-1">
            <Target className="w-4 h-4" />
            {quiz.metadata?.passing_score || 70}% to pass
          </span>
        </div>
      </div>
    </Link>
  )
}