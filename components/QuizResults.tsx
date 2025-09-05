'use client'

import { useRouter } from 'next/navigation'
import { Trophy, CheckCircle, XCircle, RotateCcw, Home } from 'lucide-react'
import type { Quiz, AnswerRecord } from '@/types'

interface QuizResultsProps {
  quiz: Quiz
  answers: AnswerRecord[]
  score: number
  totalQuestions: number
  onRetry?: () => void
}

export default function QuizResults({ 
  quiz, 
  answers, 
  score, 
  totalQuestions,
  onRetry
}: QuizResultsProps) {
  const router = useRouter()
  const correctAnswers = answers.filter(a => a.is_correct).length
  const passingScore = quiz.metadata?.passing_score || 70
  const isPassed = score >= passingScore

  const getScoreColor = () => {
    if (score >= 90) return 'text-success'
    if (score >= 70) return 'text-warning'
    return 'text-error'
  }

  const handleRetry = () => {
    if (onRetry) {
      onRetry()
    } else {
      // Fallback to reload if no onRetry handler provided
      window.location.reload()
    }
  }

  return (
    <div className="container mx-auto px-4 py-8">
      <div className="max-w-2xl mx-auto bg-white rounded-lg shadow-lg p-8">
        <div className="text-center">
          <Trophy className={`w-20 h-20 mx-auto mb-4 ${isPassed ? 'text-yellow-500' : 'text-gray-400'}`} />
          
          <h1 className="text-3xl font-bold mb-4">
            {isPassed ? 'Congratulations!' : 'Quiz Complete'}
          </h1>
          
          <div className={`text-5xl font-bold mb-2 ${getScoreColor()}`}>
            {score}%
          </div>
          
          <p className="text-gray-600 mb-6">
            You got {correctAnswers} out of {totalQuestions} questions correct
          </p>
          
          {isPassed ? (
            <div className="bg-success bg-opacity-10 border-2 border-success rounded-lg p-4 mb-6">
              <CheckCircle className="w-6 h-6 text-success inline mr-2" />
              <span className="text-success font-semibold">
                You passed! Great job mastering {quiz.title}!
              </span>
            </div>
          ) : (
            <div className="bg-error bg-opacity-10 border-2 border-error rounded-lg p-4 mb-6">
              <XCircle className="w-6 h-6 text-error inline mr-2" />
              <span className="text-error font-semibold">
                You need {passingScore}% to pass. Keep practicing!
              </span>
            </div>
          )}
        </div>

        {/* Answer Summary */}
        <div className="mt-8 pt-6 border-t">
          <h2 className="text-xl font-semibold mb-4">Answer Summary</h2>
          <div className="space-y-2">
            {quiz.metadata.questions.map((question, index) => {
              const answer = answers[index]
              const isCorrect = answer?.is_correct || false
              
              return (
                <div
                  key={question.id}
                  className="flex items-center justify-between p-3 bg-gray-50 rounded-lg"
                >
                  <span className="text-gray-700">
                    Question {index + 1}
                  </span>
                  {isCorrect ? (
                    <CheckCircle className="w-5 h-5 text-success" />
                  ) : (
                    <XCircle className="w-5 h-5 text-error" />
                  )}
                </div>
              )
            })}
          </div>
        </div>

        {/* Action Buttons */}
        <div className="flex gap-4 mt-8">
          <button
            onClick={handleRetry}
            className="flex-1 bg-primary text-white px-6 py-3 rounded-lg font-semibold hover:bg-primary-dark transition-colors flex items-center justify-center gap-2"
          >
            <RotateCcw className="w-5 h-5" />
            Retry Quiz
          </button>
          <button
            onClick={() => router.push('/quizzes')}
            className="flex-1 bg-gray-200 text-gray-700 px-6 py-3 rounded-lg font-semibold hover:bg-gray-300 transition-colors flex items-center justify-center gap-2"
          >
            <Home className="w-5 h-5" />
            Back to Quizzes
          </button>
        </div>
      </div>
    </div>
  )
}